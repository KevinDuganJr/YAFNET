<%@ Page language="c#" Codebehind="postmessage.aspx.cs" AutoEventWireup="false" Inherits="yaf.postmessage" %>
<%@ Register TagPrefix="uc1" TagName="smileys" Src="controls/smileys.ascx" %>

<script src=jscode.aspx></script>

<form runat="server">

<p class="navlinks">
	<asp:hyperlink id=HomeLink runat="server">HomeLink</asp:hyperlink>
	� <asp:hyperlink id=CategoryLink runat="server">CategoryLink</asp:hyperlink>
	� <asp:hyperlink id=ForumLink runat="server">HyperLink</asp:hyperlink>
</p>

<table class=content cellSpacing=1 cellPadding=0 width="100%" align=center>
  <tr>
    <td class=header1 align=middle colSpan=2><asp:label id=Title runat="server">Post New Topic</asp:label></td></tr>

<tr id=PreviewRow runat=server visible=false>
	<td class=postheader valign=top>Preview:</td>
	<td class=post valign=top id=PreviewCell runat=server></td>
</tr>

  <tr id=SubjectRow runat="server">
    <td class=postheader width="30%"><asp:label id=Label1 runat="server">Subject:</asp:label></td>
    <td class=post width="60%"><asp:textbox id=Subject runat="server" cssclass="edit"></asp:textbox></td></tr>
  <tr id=FromRow runat="server">
    <td class=postheader width="30%">From:</td>
    <td class=post width="60%"><asp:textbox id="From" runat="server" cssclass="edit"></asp:textbox></td></tr></TR>
  <tr id=PriorityRow runat="server">
    <td class=postheader width="30%">Priority:</td>
    <td class=post width="60%"><asp:dropdownlist id=Priority runat="server">
<asp:ListItem Value="0" Selected="True">Normal</asp:ListItem>
<asp:ListItem Value="1">Sticky</asp:ListItem>
<asp:ListItem Value="2">Announcement</asp:ListItem>
</asp:dropdownlist></td></tr>
  <tr id=CreatePollRow runat="server">
    <td class=postheader width="30%"><asp:linkbutton id=CreatePoll runat="server">Create Poll</asp:linkbutton></td>
    <td class=post width="60%">&nbsp;</td></tr>
  <tr id=PollRow1 runat="server" visible="false">
    <td class=postfooter width="30%"><em 
      >Poll Question:</em></td>
    <td class=postfooter width="60%"><asp:textbox id=Question runat="server" cssclass="edit"></asp:textbox></td></tr>
  <tr id=PollRow2 runat="server" visible="false">
    <td class=postfooter width="30%"><em>Choice 1:</em></td>
    <td class=postfooter width="60%">
<asp:TextBox id=PollChoice1 runat="server" cssclass="edit"></asp:TextBox></td></tr>
  <tr id="PollRow3" runat="server" visible=false>
    <td class=postfooter width="30%"><em>Choice 2:</em></td>
    <td class=postfooter width="60%">
<asp:TextBox id=PollChoice2 runat="server" cssclass="edit"></asp:TextBox></td></tr>
  <tr id="PollRow4" runat="server" visible=false>
    <td class=postfooter width="30%"><em>Choice 3:</em></td>
    <td class=postfooter width="60%">
<asp:TextBox id=PollChoice3 runat="server" cssclass="edit"></asp:TextBox></td></tr>
  <tr id="PollRow5" runat="server" visible=false>
    <td class=postfooter width="30%"><em>Choice 4:</em></td>
    <td class=postfooter width="60%">
<asp:TextBox id=PollChoice4 runat="server" cssclass="edit"></asp:TextBox></td></tr>
  <tr id="PollRow6" runat="server" visible=false>
    <td class=postfooter width="30%"><em>Choice 5:</em></td>
    <td class=postfooter width="60%">
<asp:TextBox id=PollChoice5 runat="server" cssclass="edit"></asp:TextBox></td></tr>
  <tr>
    <td class=postheader vAlign=top><asp:label id=Label2 runat="server">Message:</asp:label>
    <uc1:smileys runat="server" onclick="insertsmiley"></uc1:smileys>
    </td>
    <td class=post><asp:textbox id=Message runat="server" cssclass="posteditor" TextMode="MultiLine" Rows="12"></asp:textbox></td></tr>
	<tr>
		<td class=postheader>&nbsp;</td>
		<td class=post>
			<input type=button value=" B " style="font-weight:bold" onclick="makebold()">
			<input type=button value=" I " style="font-weight:bold;font-style:italic" onclick="makeitalic()">
			<input type=button value=" U " style="font-weight:bold;text-decoration:underline" onclick="makeunderline()">
			<input type=button value=" URL " onclick="makeurl()">
			<input type=button value=" QUOTE " onclick="makequote()">
		</td>
	</tr>
  <tr>
    <td align=middle colSpan=2 class=footer1>
<asp:Button id=Preview runat="server" Text="Preview"></asp:Button>&nbsp;<asp:button id=PostReply runat="server" Text="Save"></asp:button>
<asp:Button id=Cancel runat="server" Text="Cancel"></asp:Button></td></tr></table></form>
