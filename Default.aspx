<%@ Page Language="C#" AutoEventWireup="true" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/Login.aspx");
    }
</script>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="refresh" content="0;url=Admin/Login.aspx" />
    <title>AgriTech - Redirecting to Admin Portal</title>
</head>
<body>
    <p>Redirecting to <a href="Admin/Login.aspx">Admin Portal</a>...</p>
</body>
</html>
