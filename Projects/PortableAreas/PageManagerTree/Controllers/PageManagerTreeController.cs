using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework;
using Appleseed.Framework.Site.Configuration;
using System.Web.UI;
using System.Data.SqlClient;
using Appleseed.Framework.Settings.Cache;
using Appleseed.Framework.Security;
using System.Web.UI.WebControls;
using PageManagerTree.Models;
using PageManagerTree.Massive;

namespace PageManagerTree.Controllers
{
    public class PageManagerTreeController : Controller
    {
        //
        // GET: /PageManagerTree/

        public ActionResult Index()
        {
           
            return View();
        }

        protected List<PageItem> PortalPages { get; set; }

        public PortalSettings PortalSettings
        {
            get
            {
                // Obtain PortalSettings from page else Current Context else null
                return (PortalSettings)HttpContext.Items["PortalSettings"];
            }
        }


        /// <summary>
        /// The OrderPages helper method is used to reset
        ///   the display order for tabs within the portal
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void OrderPages()
        {
            var i = 1;
            this.PortalPages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);
            this.PortalPages.Sort();

            foreach (var t in this.PortalPages)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                t.Order = i;
                i += 2;

                // rewrite tab to database
                var tabs = new PagesDB();

                // 12/16/2002 Start - Cory Isakson 
                tabs.UpdatePageOrder(t.ID, t.Order);

                // 12/16/2002 End - Cory Isakson 
            }
            CurrentCache.RemoveAll("_PageNavigationSettings_");
        }


        public ActionResult Module()
        {
            return View();
        }


        public JsTreeModel[] getChildrenTree(PageItem page)
        {
            List<PageStripDetails> childPages = new PagesDB().GetPagesinPage(this.PortalSettings.PortalID, page.ID);
            int count = 0;
            List<JsTreeModel> lstTree = new List<JsTreeModel>();

            foreach (PageStripDetails childPage in childPages)
            {
                PageItem aux = new PageItem ();
                aux.ID = childPage.PageID;
                aux.Name = childPage.PageName;
                
                JsTreeModel[] childs = getChildrenTree(aux);
                JsTreeModel node = new JsTreeModel
                {
                    data = aux.Name,
                    attr = new JsTreeAttribute { id = "pjson_" + aux.ID.ToString()},
                    children = childs
                };

                lstTree.Add(node);
                count++;
            }
            JsTreeModel[] tree = lstTree.ToArray<JsTreeModel>();

            return tree;
        }

        public JsonResult GetTreeData()
        {
            List<PageItem> pages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);
            List<JsTreeModel> lstTree = new List<JsTreeModel>();

            foreach (PageItem page in pages)
            {
                if (page.NestLevel == 0)
                {
                    JsTreeModel[] child = getChildrenTree(page);
                    JsTreeModel node = new JsTreeModel {
                                                    data = page.Name,
                                                    attr = new JsTreeAttribute { id = "pjson_" + page.ID.ToString()},
                                                    children = child };
                    lstTree.Add(node);
                }
            }

            int root = 0;
            JsTreeModel rootNode = new JsTreeModel
            {
                data = "Root",
                attr = new JsTreeAttribute { id = "pjson_" + root.ToString() },
                children = lstTree.ToArray<JsTreeModel>()
            };

            return Json(rootNode);
        }


        public JsonResult remove(int id)
        {
            try
            {
                
                var tabs = new PagesDB();
                tabs.DeletePage(id);

                return Json(new {error = false});
            }
            catch (SqlException)
            {
                string errorMessage = General.GetString("TAB_DELETE_FAILED", "Failed to delete Page", this);
                return Json(new { error = true, errorMess = errorMessage });
            }
        }


        public JsonResult create(int id)
        {
            PagesDB db = new PagesDB();
            
            this.PortalPages = db.GetPagesFlat(this.PortalSettings.PortalID);
            var t = new PageItem
            {
                Name = General.GetString("TAB_NAME", "New Page Name"),
                ID = -1,
                Order = 990000
            };

            this.PortalPages.Add(t);

            var tabs = new PagesDB();
            t.ID = tabs.AddPage(this.PortalSettings.PortalID, t.Name, t.Order);

            db.UpdatePageParent(t.ID, id, this.PortalSettings.PortalID);

            this.OrderPages();
            JsonResult treeData = GetTreeData();
            return treeData;
        }


        public JsonResult edit(int id)
        {
            string dir = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID="+ id.ToString() +
                    "&mID=" + 110+ "&Alias=" + this.PortalSettings.PortalAlias + "&returntabid=" +
                    this.PortalSettings.ActiveModule);
            return Json(new { url = dir});
        }

        private List<PageItem> getPagesInLevel(int pageInlevel)
        {
            List<PageItem> pages = new List<PageItem>();

            int level = 0;
            foreach (PageItem page in this.PortalPages)
            {
                if (page.ID == pageInlevel)
                    level = page.NestLevel;
            }

            foreach (PageItem p in this.PortalPages)
            {
                if (p.NestLevel == level)
                    pages.Add(p);
            }
            return pages;
        }

        private int getPageOrder(int idToSearch)
        {
            List<PageItem> pages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);

            while (pages.Count > 0)
            {
                PageItem page = pages.First<PageItem>();
                pages.Remove(page);
                if (page.ID == idToSearch)
                {
                    return page.Order;
                }
            }
            return -1;
        }

        public void moveNode(int pageID, int newParent, int idOldNode)
        {
            PagesDB db = new PagesDB();
            this.PortalPages = db.GetPagesFlat(this.PortalSettings.PortalID);

            db.UpdatePageParent(pageID, newParent, this.PortalSettings.PortalID);
            int order;
            if (idOldNode == -1)
            {
                order = 9999;
            }
            else
            {
                order = this.getPageOrder(idOldNode) - 1;
            }

            db.UpdatePageOrder(pageID, order);
            this.OrderPages();


        }

        public JsonResult Rename(int id, string name) {

            try {
                var db = new rb_Pages();
                var page = db.Single(id);
                page.PageName = name;
                db.Update(page, page.PageID);


                //string sql = @"UPDATE rb_Pages SET [] = @0 WHERE PageID = @1";

                //object[] queryargs = { name, id };

                //var t = new .Query(sql, queryargs);
            }
            catch (Exception ) {
                return Json(new { error = true });
            }

            return Json(new { error = false }); 
        }
    
    
    }
}
