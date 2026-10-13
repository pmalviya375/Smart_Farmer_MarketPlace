<%@ Page Language="C#" AutoEventWireup="true" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("~/Farmer/Login.aspx");
    }
</script>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Redirecting...</title>
</head>
<body>
    <form id="form1" runat="server">
        <p>Redirecting to <a href="Farmer/Login.aspx">Farmer Portal</a>...</p>
    </form>
</body>
</html>
