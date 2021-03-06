// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagesDB.cs" company="--">
//   Copyright � -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Class that encapsulates all data logic necessary to add/query/delete
//   Portals within the Portal database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Xml;

    using Appleseed.Framework.Data;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// Class that encapsulates all data logic necessary to add/query/delete
    ///   Portals within the Portal database.
    /// </summary>
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    public class PagesDB
    {
        #region Constants and Fields

        /// <summary>
        /// The bool show mobile.
        /// </summary>
        private const bool BoolShowMobile = false;

        /// <summary>
        /// The int parent page id.
        /// </summary>
        private const int IntParentPageId = 0; // SP will convert to NULL if 0

        /// <summary>
        /// The str all users.
        /// </summary>
        private const string StrAllUsers = "All Users;";

        /// <summary>
        /// The str mobile page name.
        /// </summary>
        private const string StrMobilePageName = ""; // NULL NOT ALLOWED IN TABLE.

        /// <summary>
        /// The str page id.
        /// </summary>
        private const string StrPageId = "@PageID";

        /// <summary>
        /// The str portal id.
        /// </summary>
        private const string StrPortalId = "@PortalID";

        #endregion

        #region Public Methods

        /// <summary>
        /// Return the portal home page in case you are on page id = 0
        /// </summary>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        /// <returns>
        /// The home page id.
        /// </returns>
        public static int PortalHomePageId(int portalId)
        {
            // TODO: Convert to stored procedure?
            // TODO: Appleseed.Framwork.Application.Site.Pages API
            var sql =
                string.Format(
                    "Select PageID  From rb_Pages  Where (PortalID = {0}) and (ParentPageID is null) and  (PageID > 0) and ( PageOrder < 2)",
                    portalId);

            var ret = Convert.ToInt32(DBHelper.ExecuteSqlScalar<int>(sql));

            return ret;
        }

        /// <summary>
        /// The AddPage method adds a new tab to the portal.<br/>
        ///   AddPage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <returns>
        /// The add page.
        /// </returns>
        public int AddPage(int portalId, string pageName, int pageOrder)
        {
            return this.AddPage(portalId, pageName, StrAllUsers, pageOrder);
        }

        /// <summary>
        /// The AddPage method adds a new tab to the portal.<br/>
        ///   AddPage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <returns>
        /// The add page.
        /// </returns>
        public int AddPage(int portalId, string pageName, string roles, int pageOrder)
        {
            // Change Method to use new all parameters method below
            // SP call moved to new method AddPage below.
            // Mike Stone - 30/12/2004
            return this.AddPage(
                portalId, IntParentPageId, pageName, pageOrder, StrAllUsers, BoolShowMobile, StrMobilePageName);
        }

        /// <summary>
        /// The AddPage method adds a new tab to the portal.<br/>
        ///   AddPage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <param name="authorizedRoles">
        /// The authorized roles.
        /// </param>
        /// <param name="showMobile">
        /// if set to <c>true</c> [show mobile].
        /// </param>
        /// <param name="mobilePageName">
        /// Name of the mobile page.
        /// </param>
        /// <returns>
        /// The add page.
        /// </returns>
        public int AddPage(
            int portalId,
            int parentPageId,
            string pageName,
            int pageOrder,
            string authorizedRoles,
            bool showMobile,
            string mobilePageName)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_AddTab", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);

                var parameterParentPageId = new SqlParameter("@ParentPageID", SqlDbType.Int, 4) { Value = parentPageId };
                command.Parameters.Add(parameterParentPageId);

                // Fixes a missing tab name
                if (string.IsNullOrEmpty(pageName))
                {
                    pageName = "New Page";
                }

                var parameterTabName = new SqlParameter("@PageName", SqlDbType.NVarChar, 50)
                    {
                        // Fixes tab name to long
                        Value = pageName.Length > 50 ? pageName.Substring(0, 49) : pageName
                    };

                command.Parameters.Add(parameterTabName);

                var parameterTabOrder = new SqlParameter("@PageOrder", SqlDbType.Int, 4) { Value = pageOrder };
                command.Parameters.Add(parameterTabOrder);

                var parameterAuthRoles = new SqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, 256)
                    {
                        Value = authorizedRoles 
                    };
                command.Parameters.Add(parameterAuthRoles);

                var parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1) { Value = showMobile };
                command.Parameters.Add(parameterShowMobile);

                var parameterMobileTabName = new SqlParameter("@MobilePageName", SqlDbType.NVarChar, 50)
                    {
                        Value = mobilePageName 
                    };
                command.Parameters.Add(parameterMobileTabName);

                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4)
                    {
                        Direction = ParameterDirection.Output 
                    };
                command.Parameters.Add(parameterPageId);

                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }

                return (int)parameterPageId.Value;
            }
        }

        /// <summary>
        /// The DeletePage method deletes the selected tab from the portal.<br/>
        ///   DeletePage Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        public void DeletePage(int pageId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_DeleteTab", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);

                // Open the database connection and execute the command
                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// This user control will render the breadcrumb navigation for the current tab.
        ///   Ver. 1.0 - 24. Dec. 2002 - First release by Cory Isakson
        /// </summary>
        /// <param name="pageId">
        /// ID of the page
        /// </param>
        /// <returns>
        /// A list of page items.
        /// </returns>
        public List<PageItem> GetPageCrumbs(int pageId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_GetTabCrumbs", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterCrumbs = new SqlParameter("@CrumbsXML", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                sqlCommand.Parameters.Add(parameterCrumbs);

                // Execute the command
                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }

                // Build a Hash table from the XML string returned
                var crumbXml = new XmlDocument();
                crumbXml.LoadXml(parameterCrumbs.Value.ToString().Replace("&", "&amp;"));

                // Iterate through the Crumbs XML
                // Return the Crumb Page Items as an array list 
                return
                    crumbXml.FirstChild.ChildNodes.Cast<XmlNode>().Where(node => node.Attributes != null).Select(
                        node =>
                        new PageItem
                            {
                                ID = Int16.Parse(node.Attributes.GetNamedItem("tabID").Value),
                                Name = node.InnerText,
                                Order = Int16.Parse(node.Attributes.GetNamedItem("level").Value)
                            }).ToList();
            }
        }

        /// <summary>
        /// Get a pages parentID
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// The get page parent id.
        /// </returns>
        public int GetPageParentId(int portalId, int pageId)
        {
            var strSql = string.Format("rb_GetPagesParentTabID {0}, {1}", portalId, pageId);

            // Read the result set
            int parentId = Convert.ToInt32(DBHelper.ExecuteSqlScalar<int>(strSql));
            return parentId;
        }

        /// <summary>
        /// Get a pages tab order
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// The get page tab order.
        /// </returns>
        public int GetPageTabOrder(int portalId, int pageId)
        {
            var strSql = string.Format(
                "select PageOrder from rb_Pages Where (PortalID = {0}) AND (PageID = {1})", portalId, pageId);

            // Read the result set
            var tabOrder = Convert.ToInt32(DBHelper.ExecuteSqlScalar<int>(strSql));
            return tabOrder;
        }

        /// <summary>
        /// Gets the pages by portal.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A System.Data.SqlClient.SqlDataReader value...
        /// </returns>
        [Obsolete("Replace me")]
        public SqlDataReader GetPagesByPortal(int portalId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_GetTabsByPortal", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

            // Mark the Command as a SPROC
            // Add Parameters to SPROC
            var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
            sqlCommand.Parameters.Add(parameterPortalId);

            // Execute the command
            connection.Open();
            var result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader 
            return result;
        }

        /// <summary>
        /// Gets the pages flat.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A list of page items.
        /// </returns>
        public List<PageItem> GetPagesFlat(int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_GetTabsFlat", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter("@PortalID", SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);

                // Execute the command
                connection.Open();
                var result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                var desktopPages = new List<PageItem>();

                // Read the result set
                try
                {
                    while (result.Read())
                    {
                        var tabItem = new PageItem
                            {
                                ID = (int)result["PageID"],
                                Name = (string)result["PageName"],
                                Order = (int)result["PageOrder"],
                                NestLevel = (int)result["NestLevel"]
                            };
                        desktopPages.Add(tabItem);
                    }
                }
                finally
                {
                    result.Close(); // by Manu, fixed bug 807858
                }

                return desktopPages;
            }
        }

        /// <summary>
        /// Gets the pages flat table.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A data table.
        /// </returns>
        public DataTable GetPagesFlatTable(int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var sqlConnection = Config.SqlConnectionString)
            {
                var commandText = "rb_GetPageTree " + portalId;
                var da = new SqlDataAdapter(commandText, sqlConnection);

                var dataTable = new DataTable("Pages");

                // Read the result set
                try
                {
                    da.Fill(dataTable);
                }
                finally
                {
                    da.Dispose();
                }

                return dataTable;
            }
        }

        /// <summary>
        /// Gets the pages parent.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// A System.Data.SqlClient.SqlDataReader value...
        /// </returns>
        public IList<PageItem> GetPagesParent(int portalId, int pageId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var sqlCommand = new SqlCommand("rb_GetTabsParent", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

            // Mark the Command as a SPROC
            // Add Parameters to SPROC
            var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
            sqlCommand.Parameters.Add(parameterPortalId);
            var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
            sqlCommand.Parameters.Add(parameterPageId);

            // Execute the command
            connection.Open();
            var dr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

            IList<PageItem> result = new List<PageItem>();

            while (dr.Read())
            {
                var item = new PageItem { ID = Convert.ToInt32(dr["PageID"]), Name = (string)dr["PageName"] };
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Gets the pages in page.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// A System.Collections.ArrayList value...
        /// </returns>
        public List<PageStripDetails> GetPagesinPage(int portalId, int pageId)
        {
            var desktopPages = new List<PageStripDetails>();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_GetTabsinTab", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);

                // Execute the command
                connection.Open();
                var result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);

                // Read the result set
                try
                {
                    while (result.Read())
                    {
                        var tabDetails = new PageStripDetails
                            {
                                PageID = (int)result["PageID"],
                                ParentPageID = Int32.Parse("0" + result["ParentPageID"]),
                                PageName = (string)result["PageName"],
                                PageOrder = (int)result["PageOrder"],
                                AuthorizedRoles = (string)result["AuthorizedRoles"]
                            };

                        // Update the AuthorizedRoles Variable
                        desktopPages.Add(tabDetails);
                    }
                }
                finally
                {
                    result.Close(); // by Manu, fixed bug 807858
                }
            }

            return desktopPages;
        }

        /// <summary>
        /// UpdatePage Method<br/>
        ///   UpdatePage Stored Procedure
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="pageName">
        /// Name of the page.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        /// <param name="authorizedRoles">
        /// The authorized roles.
        /// </param>
        /// <param name="mobilePageName">
        /// Name of the mobile page.
        /// </param>
        /// <param name="showMobile">
        /// if set to <c>true</c> [show mobile].
        /// </param>
        public void UpdatePage(
            int portalId,
            int pageId,
            int parentPageId,
            string pageName,
            int pageOrder,
            string authorizedRoles,
            string mobilePageName,
            bool showMobile)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_UpdateTab", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterParentPageId = new SqlParameter("@ParentPageID", SqlDbType.Int, 4) { Value = parentPageId };
                sqlCommand.Parameters.Add(parameterParentPageId);

                // Fixes a missing tab name
                if (string.IsNullOrEmpty(pageName))
                {
                    pageName = "&nbsp;";
                }

                var parameterTabName = new SqlParameter("@PageName", SqlDbType.NVarChar, 50)
                    {
                        Value = pageName.Length > 50 ? pageName.Substring(0, 49) : pageName 
                    };

                sqlCommand.Parameters.Add(parameterTabName);
                var parameterTabOrder = new SqlParameter("@PageOrder", SqlDbType.Int, 4) { Value = pageOrder };
                sqlCommand.Parameters.Add(parameterTabOrder);
                var parameterAuthRoles = new SqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, 256)
                    {
                        Value = authorizedRoles 
                    };
                sqlCommand.Parameters.Add(parameterAuthRoles);
                var parameterMobileTabName = new SqlParameter("@MobilePageName", SqlDbType.NVarChar, 50)
                    {
                        Value = mobilePageName 
                    };
                sqlCommand.Parameters.Add(parameterMobileTabName);
                var parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1) { Value = showMobile };
                sqlCommand.Parameters.Add(parameterShowMobile);
                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Update Page Custom Settings
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="key">
        /// The setting key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        [Obsolete("UpdatePageCustomSettings was moved in PageSettings.UpdatePageSetting", false)]
        public void UpdatePageCustomSettings(int pageId, string key, string value)
        {
            PageSettings.UpdatePageSettings(pageId, key, value);
        }

        /// <summary>
        /// The UpdatePageOrder method changes the position of the tab with respect
        ///   to other tabs in the portal.<br/>
        ///   UpdatePageOrder Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="pageOrder">
        /// The page order.
        /// </param>
        public void UpdatePageOrder(int pageId, int pageOrder)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_UpdateTabOrder", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterTabOrder = new SqlParameter("@PageOrder", SqlDbType.Int, 4) { Value = pageOrder };
                sqlCommand.Parameters.Add(parameterTabOrder);
                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// The UpdatePageOrder method changes the position of the tab with respect
        ///   to other tabs in the portal.<br/>
        ///   UpdatePageOrder Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        public void UpdatePageParent(int pageId, int parentPageId, int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand("rb_UpdateTabParent", connection))
            {
                // Mark the Command as a SPROC
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrPortalId, SqlDbType.Int, 4) { Value = portalId };
                sqlCommand.Parameters.Add(parameterPortalId);
                var parameterPageId = new SqlParameter(StrPageId, SqlDbType.Int, 4) { Value = pageId };
                sqlCommand.Parameters.Add(parameterPageId);
                var parameterParentPageId = new SqlParameter("@ParentPageID", SqlDbType.Int, 4) { Value = parentPageId };
                sqlCommand.Parameters.Add(parameterParentPageId);

                connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion
    }
}