﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="Site.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="carousel_container" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="panel"></div>
    <input id="irEdit" type="hidden" runat="server" />
        <asp:Panel ID="LinkContainer" runat="server">
    </asp:Panel>
    <script type="text/javascript">

        function SetSessionValue(inKey, inValue) {
            $.ajax({
                type: "POST",
                url: "/SessionManager.asmx/SetSessionValue",
                data: { key: inKey, value: inValue },
                async: false,
            });
        };


        $(document).ready(function () {
            $(document).on("click", ".irLink", function (e) {
                var irGroupId = $(this).attr("irGroupId");

                SetSessionValue("EirId", irGroupId);
                e.preventDefault();
                window.open("CardView.aspx", "_self");
            });

        });
    </script>
</asp:Content>
