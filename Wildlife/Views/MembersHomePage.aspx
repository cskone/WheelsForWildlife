<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MembersHomePage.aspx.cs" Inherits="Wildlife.MembersHomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Member's Home Page</title>
    <style>
        body {
            background-color: lightblue;
        }

        div {
            text-align: right;
        }

        form {
            text-align: center;
        }

        p {
            padding: 50px 10px;
            border-style: solid;
            text-align: center;
        }
    </style>
</head>
<body>


    <div class="topnav">
        <a class="active" href="#home">Home</a>
        <a href="#preferences">Preferences</a>
        <a href="#Stats">Stats</a>
        <a href="#username">username</a>
        <a href="#logout">Logout</a>
    </div>

    <form>
        <h3 style="font-family:'Lucida Bright'">Wheels for Wildlife</h3>
        <h2 style="font-family:'Lucida Bright'">Home Page</h2>

    </form>

    <p>
        *You have no drive availability set; please set your availability in the preferences tab!
    </p>



</body>
</html>