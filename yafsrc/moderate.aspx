<%@ Page language="c#" Codebehind="moderate.aspx.cs" AutoEventWireup="false" Inherits="yaf.moderate" %>

<form runat=server>

<p class="navlinks">
	<asp:hyperlink id=HomeLink runat="server">Home</asp:hyperlink>
	� <asp:hyperlink id=CategoryLink runat="server">Category</asp:hyperlink>
	� <asp:hyperlink id=ForumLink runat="server">Forum</asp:hyperlink>
	� <asp:hyperlink id=ModLink runat="server">Moderate Forum</asp:hyperlink>
</p>

<table class=content cellspacing=1 cellpadding=0 width=100%>
<tr>
	<td class=header1 colspan=7>Moderate</td>
</tr>
<tr>
	<td class=header2 width="1%">&nbsp;</td>
	<td class=header2 align=left>Topics</td>
	<td class=header2 align=left width="20%">Topic Starter</td>
	<td class=header2 align=middle width="7%">Replies</td>
	<td class=header2 align=middle width="7%">Views</td>
	<td class=header2 align=middle width="25%">Last Post</td>
	<td class=header2>&nbsp;</td>
</tr>

<asp:repeater id=topiclist runat=server>
<itemtemplate>
	<tr class=post>
		<td><img src='<%# GetTopicImage(Container.DataItem) %>'></td>
		<td class=largefont><%# GetPriorityMessage((System.Data.DataRowView)Container.DataItem) %>
			<a href='posts.aspx?t=<%# DataBinder.Eval(Container.DataItem, "LinkTopicID") %>'><%# DataBinder.Eval(Container.DataItem, "Subject") %></a></td>
		<td><a href='profile.aspx?u=<%# DataBinder.Eval(Container.DataItem, "UserID") %>'><%# DataBinder.Eval(Container.DataItem, "Starter") %></a></td>
		<td align=center><%# DataBinder.Eval(Container.DataItem, "Replies") %></td>
		<td align=center><%# DataBinder.Eval(Container.DataItem, "Views") %></td>
		<td align=center class=smallfont><%# FormatLastPost((System.Data.DataRowView)Container.DataItem) %></td>
		<td>
			<asp:linkbutton runat=server commandargument='<%# DataBinder.Eval(Container.DataItem, "TopicID") %>' commandname='delete'><img src='<%# ThemeFile("b_delete_topic.png") %>'></asp:linkbutton>
		</td>
	</tr>
</itemtemplate>
</asp:repeater>

<tr>
	<td class=footer1 colspan=7>&nbsp;</td>
</tr>
</table>

</form>
