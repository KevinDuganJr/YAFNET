﻿/* Yet Another Forum.NET
 * Copyright (C) 2006-2010 Jaben Cargman
 * http://www.yetanotherforum.net/
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
namespace YAF.Editors
{
  using System;
  using System.Reflection;
  using System.Web.UI.WebControls;
  using YAF.Classes;
  using YAF.Classes.Core;

  #region "Telerik RadEditor"

  /// <summary>
  /// The rad editor.
  /// </summary>
  public class RadEditor : RichClassEditor
  {
    // base("Namespace,AssemblyName")
    /// <summary>
    /// Initializes a new instance of the <see cref="RadEditor"/> class.
    /// </summary>
    public RadEditor()
      : base("Telerik.Web.UI.RadEditor,Telerik.Web.UI")
    {
      InitEditorObject();
    }

    /// <summary>
    /// The on init.
    /// </summary>
    /// <param name="e">
    /// The e.
    /// </param>
    protected override void OnInit(EventArgs e)
    {
      if (this._init)
      {
        Load += new EventHandler(Editor_Load);
        base.OnInit(e);
      }
    }

    /// <summary>
    /// The editor_ load.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    protected virtual void Editor_Load(object sender, EventArgs e)
    {
      if (this._init && this._editor.Visible)
      {
        PropertyInfo pInfo = this._typEditor.GetProperty("ID");
        pInfo.SetValue(this._editor, "edit", null);
        pInfo = this._typEditor.GetProperty("Skin");

        pInfo.SetValue(this._editor, Config.RadEditorSkin, null);
        pInfo = this._typEditor.GetProperty("Height");

        pInfo.SetValue(this._editor, Unit.Pixel(400), null);
        pInfo = this._typEditor.GetProperty("Width");

        pInfo.SetValue(this._editor, Unit.Percentage(100), null);

        if (Config.UseRadEditorToolsFile)
        {
          pInfo = this._typEditor.GetProperty("ToolsFile");
          pInfo.SetValue(this._editor, Config.RadEditorToolsFile, null);
        }

        // Add Editor
        this.AddEditorControl(this._editor);

        // Register smiley JavaScript
        RegisterSmilieyScript();
      }
    }

    /// <summary>
    /// The register smiliey script.
    /// </summary>
    protected virtual void RegisterSmilieyScript()
    {
      YafContext.Current.PageElements.RegisterJsBlock(
        "InsertSmileyJs", 
        @"function insertsmiley(code,img){" + "\n" + "var editor = $find('" + this._editor.ClientID + "');" +
        "editor.pasteHtml('<img src=\"' + img + '\" alt=\"\" />');\n" + "}\n");
    }

    #region Properties

    /// <summary>
    /// Gets Description.
    /// </summary>
    public override string Description
    {
      get
      {
        return "Telerik RAD Editor (HTML)";
      }
    }

    /// <summary>
    /// Gets ModuleId.
    /// </summary>
    public override int ModuleId
    {
      get
      {
        // backward compatibility...
        return 8;
      }
    }

    /// <summary>
    /// Gets or sets Text.
    /// </summary>
    public override string Text
    {
      get
      {
        if (this._init)
        {
          PropertyInfo pInfo = this._typEditor.GetProperty("Html");
          return Convert.ToString(pInfo.GetValue(this._editor, null));
        }
        else
        {
          return string.Empty;
        }
      }

      set
      {
        if (this._init)
        {
          PropertyInfo pInfo = this._typEditor.GetProperty("Html");
          pInfo.SetValue(this._editor, value, null);
        }
      }
    }

    #endregion
  }

  #endregion
}