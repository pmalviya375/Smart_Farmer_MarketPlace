<%@ Page Language="C#" AutoEventWireup="true" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>AgriTech - Smart Farmer Marketplace</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }
        body {
            min-height: 100vh;
            background: linear-gradient(135deg, #f0fdf4 0%, #e2e8f0 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 24px;
        }
        .portal-card-container {
            width: 100%;
            max-width: 900px;
            background: #ffffff;
            border-radius: 20px;
            box-shadow: 0 20px 40px rgba(15, 23, 42, 0.08);
            border: 1px solid #e2e8f0;
            overflow: hidden;
        }
        .portal-header {
            padding: 40px 40px 24px 40px;
            text-align: center;
            border-bottom: 1px solid #f1f5f9;
        }
        .portal-logo-icon {
            width: 64px;
            height: 64px;
            border-radius: 16px;
            background: #1b6d34;
            color: #ffffff;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            font-size: 28px;
            margin-bottom: 16px;
            box-shadow: 0 8px 16px rgba(27, 109, 52, 0.25);
        }
        .portal-header h1 {
            font-size: 26px;
            font-weight: 800;
            color: #0f172a;
            margin-bottom: 8px;
        }
        .portal-header p {
            font-size: 14px;
            color: #64748b;
        }
        .portal-options-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 24px;
            padding: 36px 40px;
        }
        @media (max-width: 768px) {
            .portal-options-grid {
                grid-template-columns: 1fr;
                padding: 24px 20px;
            }
        }
        .portal-option-box {
            border: 2px solid #e2e8f0;
            border-radius: 16px;
            padding: 32px 24px;
            text-decoration: none;
            display: flex;
            flex-direction: column;
            align-items: center;
            text-align: center;
            transition: all 0.25s ease;
            position: relative;
            background: #ffffff;
        }
        .portal-option-box:hover {
            transform: translateY(-4px);
            box-shadow: 0 12px 24px rgba(15, 23, 42, 0.06);
        }
        .portal-option-box.farmer:hover {
            border-color: #1b6d34;
        }
        .portal-option-box.admin:hover {
            border-color: #2563eb;
        }
        .box-icon {
            width: 56px;
            height: 56px;
            border-radius: 14px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 24px;
            margin-bottom: 18px;
        }
        .farmer .box-icon {
            background: #ecfdf5;
            color: #1b6d34;
        }
        .admin .box-icon {
            background: #eff6ff;
            color: #2563eb;
        }
        .portal-option-box h2 {
            font-size: 18px;
            font-weight: 700;
            color: #0f172a;
            margin-bottom: 8px;
        }
        .portal-option-box p {
            font-size: 13px;
            color: #64748b;
            line-height: 1.5;
            margin-bottom: 20px;
            flex-grow: 1;
        }
        .portal-action-btn {
            width: 100%;
            padding: 10px 20px;
            border-radius: 10px;
            font-size: 13.5px;
            font-weight: 600;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            transition: all 0.2s ease;
        }
        .farmer .portal-action-btn {
            background: #1b6d34;
            color: #ffffff;
        }
        .farmer:hover .portal-action-btn {
            background: #14532d;
        }
        .admin .portal-action-btn {
            background: #2563eb;
            color: #ffffff;
        }
        .admin:hover .portal-action-btn {
            background: #1d4ed8;
        }
        .portal-footer {
            padding: 18px 40px;
            background: #f8fafc;
            border-top: 1px solid #f1f5f9;
            text-align: center;
            font-size: 12px;
            color: #94a3b8;
        }
    </style>
</head>
<body>
    <div class="portal-card-container">
        <div class="portal-header">
            <div class="portal-logo-icon">
                <i class="fa-solid fa-seedling"></i>
            </div>
            <h1>Smart Farmer Marketplace</h1>
            <p>Select your portal to continue</p>
        </div>

        <div class="portal-options-grid">
            <!-- FARMER PORTAL OPTION -->
            <a href="Farmer/Login.aspx" class="portal-option-box farmer">
                <div class="box-icon">
                    <i class="fa-solid fa-tractor"></i>
                </div>
                <h2>Farmer Portal</h2>
                <p>Manage crops, agricultural equipment, livestock listings, buyer order requests, and crop categories.</p>
                <div class="portal-action-btn">
                    <span>Enter Farmer Portal</span>
                    <i class="fa-solid fa-arrow-right"></i>
                </div>
            </a>

            <!-- ADMIN PORTAL OPTION -->
            <a href="Admin/Login.aspx" class="portal-option-box admin">
                <div class="box-icon">
                    <i class="fa-solid fa-shield-halved"></i>
                </div>
                <h2>Admin Enterprise Portal</h2>
                <p>Oversee marketplace growth, manage users, moderate products, review orders, categories, and support queries.</p>
                <div class="portal-action-btn">
                    <span>Enter Admin Portal</span>
                    <i class="fa-solid fa-arrow-right"></i>
                </div>
            </a>
        </div>

        <div class="portal-footer">
            AgriTech Smart Farmer Marketplace System &copy; 2026. Built with ASP.NET &amp; SQL Server.
        </div>
    </div>
</body>
</html>
