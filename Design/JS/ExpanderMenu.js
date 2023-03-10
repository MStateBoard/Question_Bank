/************************************************************************************************
Name		- ExpanderMenu Script
Description	- Provides client-side functionalities for ExpanderMenu control.
Version		- 2.2.2
Author		- Anoop Nair.
Company		- MKCL.

# COPYRIGHT NOTICE
# Copyright (c) 2005-2010 MKCL, All rights reserved.
# This script may be used and modified free of charge for Non-profit purposes by anyone as long
# as this copyright notice and the comments above are kept in their original form.
************************************************************************************************/

if (typeof (Synthesys) == 'undefined')
    window.reload(true);

Synthesys.ExpanderMenu = new function () {
    this.ProjectName = 'Synthesys';
    this.SlideDelay = 5;
    this.SlidePixel = 5;
    this.DoFading = false;
    this.FrameName = '';
    var images = new function () {
        this.upNorm = new Image();
        this.upGlow = new Image();
        this.dnNorm = new Image();
        this.dnGlow = new Image();
        this.close = new Image();
    };

    var timerSlide = null, isLocked = null, numMenuItem = 0;
    var data = new Array(), state = new Array(), regMenus = new Array();
    data[0] = new Array();
    data[1] = new Array();
    data[2] = new Array();
    data[3] = new Array();
    data[4] = new Array();
    data[5] = new Array();
    data[6] = new Array();
    data[7] = new Array();
    data[8] = new Array();
    data[9] = new Array();
    //images.upNorm.src = "../SynthesysModules_Files/Images/ExpanderMenu_up.png";
    //images.upGlow.src = "../SynthesysModules_Files/Images/ExpanderMenu_upgw.png";
    //images.dnNorm.src = "../SynthesysModules_Files/Images/ExpanderMenu_down.png";
    //images.dnGlow.src = "../SynthesysModules_Files/Images/ExpanderMenu_downgw.png";
    //images.close.src = "../Images/ExpanderMenu_close.gif";

    this.InitializeDynamicMenu = function (MenuGroupId) {
        var MenuGroup = document.getElementById('MenuGroup_' + MenuGroupId);
        var Links = MenuGroup.getElementByTagName('A');
    };
    this.SetSelectedLink = function (MenuGroupId, MenuLinkId) {
        var MenuGroup = document.getElementById('MenuGroup_' + MenuGroupId);
        var MenuLink = MenuGroup.getElementById('MenuLink_' + MenuLinkId);
        if (MenuGroup.LinkSelected == MenuLinkId)
            return;
        Synthesys.ExpanderMenu.EnableDynamicLink(MenuGroup, MenuLink);
        link.className = link.className + ' Selected';
        var oldSelected = MenuGroup.getElementById('MenuLink_' + MenuGroup.LinkSelected);
        oldSelected.className = oldSelected.className.replace(' Selected', '');
        MenuGroup.LinkSelected = MenuLinkId;
    };
    this.RegisterMenu = function (MenuId, disLinks) {
        regMenus[regMenus.length] = new Object();
        regMenus[regMenus.length - 1].MenuId = MenuId;
        regMenus[regMenus.length - 1].LinkDisbl = disLinks;

        if (document.getElementById('ctl00_TopMenu') != null) {
            if (document.getElementById('MenuGroup_0') != null) {
                document.getElementById('MenuGroup_0').style.display = '';
                if (document.getElementById('MenuGroup_0').nextSibling != null) {
                    document.getElementById('MenuGroup_0').nextSibling.style.display = '';
                }
            }
        }
    };
    this.ChangeStyle = function () {
        var ShrinkButton = this.getElementsByTagName('img').item(0);
        if (ShrinkButton.src.EndsWith("upgw.png"))
            ShrinkButton.src = images.upNorm.src;
        else if (ShrinkButton.src.EndsWith("up.png"))
            ShrinkButton.src = images.upGlow.src;
        else if (ShrinkButton.src.EndsWith("downgw.png"))
            ShrinkButton.src = images.dnNorm.src;
        else if (ShrinkButton.src.EndsWith("down.png"))
            ShrinkButton.src = images.dnGlow.src;
        if (document.body.style.cursor == 'pointer')
            document.body.style.cursor = 'default';
        else document.body.style.cursor = 'pointer';
    };
    this.SetSlide = function () {
        if (isLocked)
            return;
        else
            isLocked = this.parentNode;
        for (i = 0; i < data[0].length; i++) {
            if (data[3][i] == this.parentNode) {
                if (data[5][i] == null)
                    data[5][i] = setInterval("Synthesys.ExpanderMenu.RunSlide(" + i + ")",
						Synthesys.ExpanderMenu.SlideDelay);
                break;
            }
        }
    };
    this.RunSlide = function (menuIndex) {
        if (data[6][menuIndex]) {
            if (Synthesys.ExpanderMenu.DoFading) {
                if (data[0][menuIndex].filter)
                    data[0][menuIndex].filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity=" +
						(parseInt(data[0][menuIndex].filter) - (100 / (
							(data[8][menuIndex] / Synthesys.ExpanderMenu.SlidePixel) + 1))) + ")";
                else if (data[0][menuIndex].filters.alpha)
                    data[0][menuIndex].filters.alpha.opacity -=
						100 / ((data[8][menuIndex] / Synthesys.ExpanderMenu.SlidePixel) + 1);
                else
                    data[0][menuIndex].style.opacity -=
						.9 / ((data[8][menuIndex] / Synthesys.ExpanderMenu.SlidePixel) + 1);
            }
            data[4][menuIndex] -= Synthesys.ExpanderMenu.SlidePixel;
            if (data[4][menuIndex] > 0) {
                data[0][menuIndex].style.height = data[4][menuIndex] + "px";
            }
            else {
                if (Synthesys.ExpanderMenu.DoFading) {
                    if (data[0][menuIndex].filter)
                        data[0][menuIndex].filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                    else if (data[0][menuIndex].filters.alpha)
                        data[0][menuIndex].filters.alpha.opacity = 0;
                    else
                        data[0][menuIndex].style.opacity = 0;
                }
                data[4][menuIndex] = 0;
                data[0][menuIndex].style.height = 0 + "px";
                clearInterval(data[5][menuIndex]);
                data[5][menuIndex] = null;
                data[6][menuIndex] = false;
                state[parseInt(data[9][i])] = 1;
                isLocked = null;
                UpdateCookie();
                var ImageButton = data[7][i].getElementsByTagName('img').item(0);
                if (ImageButton.src.substring(ImageButton.src.length - 6, ImageButton.src.length - 4) == "up")
                    ImageButton.src = images.dnNorm.src;
                else
                    ImageButton.src = images.dnGlow.src;
                return 0;
            }
            return 0;
        }
        if (!data[6][menuIndex]) {
            if (Synthesys.ExpanderMenu.DoFading) {
                if (data[0][menuIndex].filter) {
                    data[0][menuIndex].filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity="
						+ (parseInt(data[0][menuIndex].filter)
						+ (100 / ((data[8][menuIndex] / Synthesys.ExpanderMenu.SlidePixel) + 1))) + ")";
                }
                else if (data[0][menuIndex].filters.alpha) {
                    data[0][menuIndex].filters.alpha.opacity +=
						100 / ((data[8][menuIndex] / Synthesys.ExpanderMenu.SlidePixel) + 1);
                }
                else {
                    opcVal = parseFloat(data[0][menuIndex].style.opacity);
                    opcVal += .9 / (data[8][menuIndex] / Synthesys.ExpanderMenu.SlidePixel);
                    data[0][menuIndex].style.opacity = opcVal;
                }
            }
            data[4][menuIndex] += Synthesys.ExpanderMenu.SlidePixel;
            if (data[4][menuIndex] < data[8][menuIndex]) {
                data[0][menuIndex].style.height = data[4][menuIndex] + "px";
            }
            else {
                if (Synthesys.ExpanderMenu.DoFading) {
                    if (data[0][menuIndex].filter)
                        data[0][menuIndex].filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                    else if (data[0][menuIndex].filters.alpha)
                        data[0][menuIndex].filters.alpha.opacity = 100;
                    else
                        data[0][menuIndex].style.opacity = 1;
                }
                data[4][menuIndex] = data[8][menuIndex];
                data[0][menuIndex].style.height = data[8][menuIndex] + "px";
                clearInterval(data[5][menuIndex]);
                data[5][menuIndex] = null;
                data[6][menuIndex] = true;
                state[parseInt(data[9][i])] = 0;
                isLocked = null;
                UpdateCookie();
                var ImageButton = data[7][i].getElementsByTagName('img').item(0);
                if (ImageButton.src.substring(ImageButton.src.length - 6, ImageButton.src.length - 4) == "wn")
                    ImageButton.src = images.upNorm.src;
                else
                    ImageButton.src = images.upGlow.src;
                return 0;
            }
            return 0;
        }
    };
    this.InitAllMenus = function () {
        for (var i = 0; i < regMenus.length; i++) {
            var disLinks = regMenus[i].LinkDisbl;
            var allLinks = document.getElementById(regMenus[i].MenuId).getElementsByTagName('A');
            for (var j = 0; j < disLinks.length; j++) {
                for (var k = 0; k < allLinks.length; k++) {
                    if (allLinks[k].parentNode.id.substring(9) == disLinks[j][0]) {
                        if (disLinks[j][1] == 2)
                            DisableLink(allLinks[k].parentNode);
                        else if (disLinks[j][1] == 3)
                            HideLink(allLinks[k].parentNode);
                    }
                }
            }
            var allMenus = document.getElementById(regMenus[i].MenuId).childNodes;
            for (var j = 0; j < disLinks.length; j++) {
                for (var k = 0; k < allMenus.length; k++) {
                    if (allMenus[k].nodeName == 'DIV' && allMenus[k].id.substring(10) == disLinks[j][0]) {
                        if (disLinks[j][1] == 0)
                            DisableGroup(allMenus[k]);
                        else if (disLinks[j][1] == 1) {
                            HideGroup(allMenus[k]);
                        }
                    }
                }
            }
            if (Synthesys.TopMenu.SelectedMenus) {
                var hideThis = true;
                for (var k = 0; k < allMenus.length; k++) {
                    if (allMenus[k].nodeName != 'DIV')
                        continue;
                    hideThis = true;
                    for (var j = 0; j < Synthesys.TopMenu.SelectedMenus.length; j++) {
                        if (allMenus[k].id.substring(10) == Synthesys.TopMenu.SelectedMenus[j]) {
                            hideThis = false;
                            break;
                        }
                    }
                    //                    if (!hideThis)
                    //                        HideGroup(allMenus[k]);
                }
            }
            InitMenuCollection(regMenus[i].MenuId);
        }
    };

    function EnableLink(MenuLink, DynamicLink) {
        if (typeof (MenuLink.LinkEnabled) != 'undefined' && MenuLink.LinkEnabled)
            return;
        var anchor = MenuLink.getElementsByTagName('A')[0];
        if (typeof (DynamicLink) == 'undefined' || !DynamicLink)
            anchor.href = anchor.link;
        else
            anchor.href = 'javascript:Synthesys.ExpanderMenu.LoadLink(this.link);';
        MenuLink.className = MenuLink.className.replace(' Disabled', '');
        MenuLink.LinkEnabled = true;
    }
    function DisableLink(MenuLink) {
        if (typeof (MenuLink.LinkEnabled) != 'undefined' && !MenuLink.LinkEnabled)
            return;
        MenuLink.getElementsByTagName('A')[0].href = 'javascript:void(0);';
        MenuLink.className = MenuLink.className + ' Disabled';
        MenuLink.LinkEnabled = false;
    }
    function EnableGroup(MenuGroup) {
        if (typeof (MenuGroup.GroupEnabled) != 'undefined' && MenuGroup.GroupEnabled)
            return;
        var anchor = MenuLink.getElementsByTagName('A')[0];
        if (typeof (DynamicLink) == 'undefined' || !DynamicLink)
            anchor.href = anchor.link;
        else
            anchor.href = 'javascript:Synthesys.ExpanderMenu.LoadLink(this.link);';
        MenuGroup.className = MenuLink.className.replace(' Disabled', '');
        MenuGroup.GroupEnabled = true;
    }
    function DisableGroup(MenuGroup) {
        var links = MenuGroup.getElementsByTagName('DIV')[1].childNodes[0].childNodes;
        for (var i = 0; i < links.length; i++)
            if (links[i].nodeName == 'DIV')
                DisableLink(links[i]);
    }
    function HideLink(MenuLink) {
        if (typeof (MenuLink.LinkVisible) != 'undefined' && !MenuLink.LinkVisible)
            return;
        //MenuLink.getElementsByTagName('A')[0].href = 'javascript:void(0);';
        //MenuLink.className = MenuLink.className + ' Disabled';
        MenuLink.style.display = 'none';
        MenuLink.LinkVisible = false;
    }
    function HideGroup(MenuGroup) {
        if (typeof (MenuGroup.GroupVisible) != 'undefined' && !MenuGroup.GroupVisible)
            return;
        //		MenuGroup.style.display = 'none';
        //		MenuGroup.nextSibling.style.display = 'none';
        MenuGroup.style.display = '';
        MenuGroup.nextSibling.style.display = '';
        MenuGroup.GroupVisible = false;
    }
    function LoadLink(LinkUrl) {
        frames[Synthesys.ExpanderMenu.FrameName].src = LinkUrl;
    }
    function InitMenuCollection(MenuId) {
        var divs = document.getElementById(MenuId).getElementsByTagName("DIV");
        GetCookie();
        for (dn = 0; dn < divs.length; dn++) {
            if (String(divs.item(dn).parentNode.className).substring(0, 6) == "Style2")
                continue;
            if (String(divs.item(dn).className).substring(0, 8) == "MenuHead") {
                mainMenuDiv = divs.item(dn).parentNode;
                menuContainerDiv = mainMenuDiv.getElementsByTagName("DIV").item(1);
                itemContainerDiv = menuContainerDiv.getElementsByTagName("DIV").item(0);
                if (Synthesys.ExpanderMenu.DoFading) {
                    if (menuContainerDiv.filter)
                        menuContainerDiv.filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                    else if (menuContainerDiv.filters.alpha)
                        menuContainerDiv.filters.alpha.opacity = 100;
                    else
                        menuContainerDiv.opacity = 1;
                }
                InitSingleMenu(divs.item(dn));
            }
        }
    }
    function InitSingleMenu(objDiv) {
        if (isLocked)
            return;
        var mainMenuDiv, MenuHeadDiv, LinkCntrDiv, LinkBoxDiv, styleRules;
        for (r = 0; r < document.styleSheets.length; r++) {
            if (-1 != String(document.styleSheets[r].href).indexOf("ExpanderMenu.css"))
                break;
        }
        if (!document.styleSheets[r].rules)
            styleRules = document.styleSheets[r].cssRules;
        else
            styleRules = document.styleSheets[r].rules;
        numMenuItem = 0;
        mainMenuDiv = objDiv.parentNode;
        MenuHeadDiv = mainMenuDiv.getElementsByTagName("DIV").item(0);
        LinkCntrDiv = mainMenuDiv.getElementsByTagName("DIV").item(1);
        LinkBoxDiv = LinkCntrDiv.getElementsByTagName("DIV").item(0);
        aLen = data[0].length;
        for (i = 0; i < aLen; i++) {
            if (data[0][i] == LinkCntrDiv)
                break;
        }
        if (i == aLen) {
            data[0][i] = LinkCntrDiv;
            data[1][i] = LinkBoxDiv;
            data[7][i] = MenuHeadDiv;
            data[9][i] = mainMenuDiv.id.substring(10);
            data[7][i].onmouseover = Synthesys.ExpanderMenu.ChangeStyle;
            data[7][i].onmouseout = Synthesys.ExpanderMenu.ChangeStyle;
            MenuHeadDiv.onclick = Synthesys.ExpanderMenu.SetSlide;
            lastmenuNum = -1;
            for (b = 0; b < LinkBoxDiv.childNodes.length; b++) {
                if (LinkBoxDiv.childNodes.item(b).tagName == "DIV") {
                    numMenuItem++;
                    lastmenuNum = b;
                }
            }
            data[2][i] = numMenuItem;
            data[3][i] = mainMenuDiv;
            data[4][i] = data[1][i].parentNode.offsetHeight;
            data[0][i].style.height = data[1][i].parentNode.offsetHeight + "px";
            data[6][i] = true;
            data[8][i] = data[1][i].parentNode.offsetHeight;
            if (Synthesys.ExpanderMenu.DoFading) {
                if (data[0][i].filter)
                    data[0][i].filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                else if (data[0][i].filters.alpha)
                    data[0][i].filters.alpha.opacity = 100;
                else
                    data[0][i].style.opacity = 1;
            }
            if (state[parseInt(data[9][i])] == 1) {
                data[7][i].getElementsByTagName('img').item(0).src = images.dnNorm.src;
                data[4][i] = 0;
                data[0][i].style.height = 0 + "px";
                data[6][i] = false;
                if (Synthesys.ExpanderMenu.DoFading) {
                    if (data[0][i].filter)
                        data[0][i].filter = "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                    else if (data[0][i].filters.alpha)
                        data[0][i].filters.alpha.opacity = 0;
                    else
                        data[0][i].style.opacity = .0;
                }
            }
        }
        mainMenuDiv = null;
        MenuHeadDiv = null;
        LinkCntrDiv = null;
        LinkBoxDiv = null;
    }
    function UpdateCookie() {
        var strName = Synthesys.ExpanderMenu.ProjectName + "_ExpanderMenu";
        date = new Date();
        date.setTime(date.getTime() + (1000 * 60 * 60 * 24 * 30));
        document.cookie = strName + "=" + escape(state.toString()) + ";expires=" + date.toGMTString()
			+ ";path=/";
    }
    function GetCookie() {
        state = "";
        var strName = Synthesys.ExpanderMenu.ProjectName + "_ExpanderMenu";
        var cookieList = document.cookie.split(";");
        for (i = 0; i < cookieList.length; i++) {
            pair = cookieList[i].split("=");
            if (pair[0] == strName) {
                state = unescape(pair[1]).split(",");
                break;
            }
        }
    }
};

Synthesys.FuncLib.AddLoadEvent(Synthesys.ExpanderMenu.InitAllMenus);

Synthesys.TopMenu = new function() {
    this.SelectedMenus = null;
    this.TopMenuFirstLinkId = null;
    this.RegisterMenu = function(MenuId) {
        var menu = document.getElementById(MenuId);
    }
    this.SetSelected = function(TopMenuId) {
        if (!document.getElementById('TopMenuLink_' + TopMenuId))
            var menuGrp = document.getElementById('TopMenuLink_' + this.TopMenuFirstLinkId);
        else
            var menuGrp = document.getElementById('TopMenuLink_' + TopMenuId);
        if (menuGrp) {
            //menuGrp.parentNode.className += ' Selected';
            menuGrp.className += ' Selected';   //jayesh
            menuGrp.href = '#';
            Synthesys.TopMenu.SelectedMenus = (typeof (menuGrp.leftmenus) == 'undefined' ?
			    menuGrp.getAttribute('leftmenus').split(',') : menuGrp.leftmenus.split(','));
        }
    };
};
