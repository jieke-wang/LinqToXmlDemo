﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="LinqToXmlDemo.Demo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css" media="all">
    table
    {
        border:0px;
        margin:1px 1px 1px 1px;
    }
    
    table th
    {
        background-color:#c8c8c8;
    }
    
    table td
    {
        background-color:#eeeeee;
    }
    
    .blueButton { background-color:Background;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ListView ID="lvTest" runat="server" DataKeyNames="ID" OnItemCanceling="lvTest_ItemCanceling"
            OnItemDeleting="lvTest_ItemDeleting" OnItemEditing="lvTest_ItemEditing" OnItemUpdating="lvTest_ItemUpdating">
            <LayoutTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>
                                ID
                            </th>
                            <th>
                                姓名
                            </th>
                            <th>
                                年龄
                            </th>
                            <th>
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tfoot>
                        <tr>
                            <th>
                                ID
                            </th>
                            <th>
                                姓名
                            </th>
                            <th>
                                年龄
                            </th>
                            <th>
                                操作
                            </th>
                        </tr>
                    </tfoot>
                    <tbody>
                        <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("ID")%>
                    </td>
                    <td>
                        <%# Eval("Name")%>
                    </td>
                    <td>
                        <%# Eval("Age")%>
                    </td>
                    <td>
                        <asp:Button ID="btnEdit" runat="server" Text="编辑" CommandName="Edit" /><br />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" CommandName="Delete" CommandArgument='<%# Eval("ID")%>'
                            OnClientClick="return confirm('你确认要删除此项吗？');" />
                    </td>
                </tr>
            </ItemTemplate>
            <EditItemTemplate>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <th>
                                    ID
                                </th>
                                <td>
                                    <%# Eval("ID")%>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    姓名
                                </th>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name")%>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    年龄
                                </th>
                                <td>
                                    <asp:TextBox ID="txtAge" runat="server" Text='<%# Eval("Age")%>'></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    操作
                                </th>
                                <td>
                                    <asp:Button ID="btnUpdate" runat="server" Text="更新" CommandName="Update" CommandArgument='<%# Eval("ID")%>' /><br />
                                    <asp:Button ID="btnCancel" runat="server" Text="取消" CommandName="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </EditItemTemplate>
            <EmptyDataTemplate>
                <p>
                    未找到可读数据！</p>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
    <div>
        <table>
            <tr>
                <th>
                    姓名
                </th>
                <td>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    年龄
                </th>
                <td>
                    <asp:TextBox ID="txtAge" runat="server" Text="0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="brnAdd" runat="server" Text="添加" OnClick="brnAdd_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
