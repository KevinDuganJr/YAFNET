/* Copyright (C) 2003 Bj�rnar Henden
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace yaf.admin
{
	/// <summary>
	/// Summary description for repair.
	/// </summary>
	public class repair : BasePage
	{
		protected System.Web.UI.WebControls.LinkButton RepairAccess;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsAdmin) Response.Redirect(BaseDir);
			TopMenu = false;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.RepairAccess.Click += new System.EventHandler(this.RepairAccess_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void RepairAccess_Click(object sender, System.EventArgs e) {
			using(SqlCommand cmd = new SqlCommand("yaf_forumaccess_repair")) {
				cmd.CommandType = CommandType.StoredProcedure;
				DataManager.ExecuteNonQuery(cmd);
			}
			AddLoadMessage("User rights should be repaired.");
		}
	}
}
