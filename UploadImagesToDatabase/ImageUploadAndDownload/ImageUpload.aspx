<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageUpload.aspx.cs" Inherits="UploadImagesToDatabase.ImageUploadAndDownload.ImageUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <br /><br />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <br /><br />
        <asp:HyperLink ID="hyperlink" runat="server">View Uploaded Image</asp:HyperLink>
        <br /><br/>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id"/>
                <asp:BoundField DataField="Name" HeaderText="Name"/>
                <asp:BoundField DataField="Size" HeaderText="Size (byte)"/>
                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <asp:Image ID="Image1" Height="70px" Width="100px" runat="server" ImageUrl=
                            '<%#"Data:Image/png;base64,"+Convert.ToBase64String((byte[])Eval("ImageData")) %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
