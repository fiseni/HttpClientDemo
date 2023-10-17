<%@ Page Async="true" Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HttpClientDemo.WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">ASP.NET</h1>
        </section>
    <div class="row">
            <section class="col-md-4" aria-labelledby="hostingTitle">
                <p>
                    <asp:Label ID="Label1" runat="server" Text="Option"></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="TextBoxOption" runat="server" Width="159px"></asp:TextBox>
                    <asp:Button ID="ButtonRunAsync" runat="server" OnClick="ButtonRunAsync_Click" Text="Run Async" />
                    <asp:Button ID="ButtonRun" runat="server" OnClick="ButtonRun_Click" Text="Run Option63 (sync)" Width="163px" />
                </p>
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Response"></asp:Label>
                </p>
                <p>
                    <asp:TextBox ID="TextBoxResponse" runat="server"></asp:TextBox>
                </p>
                <p>
                    &nbsp;</p>
            </section>
        </div>
    </main>

</asp:Content>
