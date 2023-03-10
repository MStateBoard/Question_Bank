/************************************************************************************************
Name		- Function Library
Description	- Cross-browser advanced javascript functionalities commonly used..
Version		- 2.0.2
Author		- Anoop Nair.
Company		- Synthesys (part of MKCL).

# COPYRIGHT NOTICE
# Copyright (c) 2005-2010 MKCL, All rights reserved.
# This script may be used and modified free of charge for Non-profit purposes by anyone as long
# as this copyright notice and the comments above are kept in their original form.
************************************************************************************************/

Function.prototype.method = function(name, func)
{
	this.prototype[name] = func;
	return this;
};

String.method('RemoveAt', function(index)
{
	return this.substring(0, index) + this.substring(index + 1, this.length);
});
String.method('TrimLeft', function()
{
	var output = '';
	var c_tab = String.fromCharCode(9);
	var c_feed = String.fromCharCode(10);
	var c_return = String.fromCharCode(13);
	var c_space = String.fromCharCode(32);
	var c_nb_space = String.fromCharCode(160);
	for (i = 0; i < this.length; i++)
	{
		var temp = this.charAt(i);
		if (temp != c_tab && temp != c_feed && temp != c_return && temp != c_space && temp != c_nb_space)
			break;
	}
	return this.substring(i, this.length);
});
String.method('TrimRight', function()
{
	var output = '';
	var c_tab = String.fromCharCode(9);
	var c_feed = String.fromCharCode(10);
	var c_return = String.fromCharCode(13);
	var c_space = String.fromCharCode(32);
	var c_nb_space = String.fromCharCode(160);
	for (i = this.length - 1; i >= 0; i--)
	{
		var temp = this.charAt(i);
		if (temp != c_tab && temp != c_feed && temp != c_return && temp != c_space && temp != c_nb_space)
			break;
	}
	return this.substring(0, i + 1);
});
String.method('TrimBoth', function()
{
	return this.TrimLeft().TrimRight();
});
String.method('TrimAll', function(retainSpace)
{
	var str = this;
	var c_tab = String.fromCharCode(9);
	var c_feed = String.fromCharCode(10);
	var c_return = String.fromCharCode(13);
	var c_space = String.fromCharCode((retainSpace) ? 160 : 32);
	var c_nb_space = String.fromCharCode(160);
	for (i = 0; i < str.length; i++)
	{
		var temp = str.charAt(i);
		if (temp == c_tab || temp == c_feed || temp == c_return || temp == c_space || temp == c_nb_space)
			str = str.RemoveAt(i--);
	}
	return str;
});
String.method('StartsWith', function(input)
{
	return (this.match("^" + input) == input);
});
String.method('EndsWith', function(input)
{
	return (this.match(input + "$") == input);
});
String.method('Contains', function(input)
{
	return (this.indexOf(input) != -1);
});

function RegisterNamespace(nmSpace)
{
	var nmSplit = nmSpace.split('.');
	var obj = window;
	for (var i = 0; i < nmSplit.length; i++)
	{
		if (typeof (obj[nmSplit[i]]) == 'undefined')
			obj[nmSplit[i]] = new Object();
		obj = obj[nmSplit[i]];
	}
}
RegisterNamespace('Synthesys');

Synthesys.Point = function(x, y)
{
	this.X = x ? x : 0;
	this.Y = y ? y : 0;
	this.Add = function(othPoint)
	{
		this.X += othPoint.X;
		this.Y += othPoint.Y;
	};
	this.Subtract = function(othPoint)
	{
		this.X -= othPoint.X;
		this.Y -= othPoint.Y;
	};
};

Synthesys.ErrorHandler = function(moduleName, errorTypes, noOfLines)
{
	var mod = moduleName, lines = (noOfLines ? noOfLines : 25), err = errorTypes;
	this.ShowAlert = function(message, errorTypeId, functionName)
	{
		/*if (!functionName && typeof (this.showAlert.caller) != 'undefined')
			functionName = this.showAlert.caller.toString().match(/function\s*\(.*\)/i);*/
		var msg = '' + mod + ' Error:-\r\n';
		for (var i = 0; i < lines; i++)
			msg += '-';
		msg += '\r\n' + (functionName ? functionName + '- ' : '') + err[errorTypeId]
			+ 'Exception.\r\n\t- ' + message;
		alert(msg);
	};
};

Synthesys.FuncLib = new function()
{
	this.GetOffsetPosition = function(element)
	{
		if (!element.offsetParent)
			return new Synthesys.Point();
		var pt = new Synthesys.Point(element.offsetLeft, element.offsetTop);
		pt.Add(Synthesys.FuncLib.GetOffsetPosition(element.offsetParent));
		return pt;
	};
	this.GetInnerText = function(element)
	{
		if (typeof (element) == "undefined" || typeof (element) == "string")
			return element;
		if (element.innerText && element.innerText.TrimBoth() != '')
			return element.innerText.TrimBoth();
		else if (element.textContent && element.textContent.TrimBoth() != '')
			return element.textContent.TrimBoth();
		else if (element.innerHTML && element.innerHTML.search(/<input/i) == -1)
			return element.innerHTML.replace(/<[^>]+>/g, "");
		else if (element.nodeName == 'INPUT' && element.type == 'text')
			return element.value.TrimBoth();
		else
		{
			var textData = '';
			for (var i = 0; i < element.childNodes.length; i++)
			{
				if (element.childNodes[i].nodeType == 1)
					textData += Synthesys.FuncLib.GetInnerText(element.childNodes[i]);
				else if (element.childNodes[i].nodeType == 3)
					textData += element.childNodes[i].nodeValue;
			}
			return textData.TrimBoth();
		}
		return '';
	};
	this.GetEvent = function(e)
	{
		if (typeof (e) == 'undefined' || !e)
			e = window.event;
		e.SourceElement = e.target || e.srcElement;
		if (e.SourceElement.nodeType == 3)
			e.SourceElement = e.SourceElement.parentNode;
		e.TargetElement = e.relatedTarget || e.fromElement;
		if (e.TargetElement == e.SourceElement && e.toElement)
			e.TargetElement = e.toElement;
		if (e.pageX)
		{
			e.PositionX = e.pageX;
			e.PositionY = e.pageY;
		}
		else if (e.clientX)
		{
			e.PositionX = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
			e.PositionY = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
		}
		if (typeof (event) != 'undefined')
			e.Suppress = function() { e.returnValue = false; };
		else
			e.Suppress = function() { e.preventDefault(); };
		return e;
	};
	this.AddLoadEvent = function(functionName, useCapture)
	{
		this.AddEventHandler(window, 'load', functionName, useCapture);
	};
	this.AddEventHandler = function(element, eventType, functionName, useCapture)
	{
		if (element.AddEventHandler)
		{
			return element.AddEventHandler(eventType, functionName, useCapture);
		}
		else if (element.addEventListener)
		{
			element.addEventListener(eventType, functionName, useCapture);
			return true;
		}
		else if (element.attachEvent)
		{
			return element.attachEvent('on' + eventType, functionName);
		}
	};
	this.RemoveEventHandler = function(element, eventType, functionName, useCapture)
	{
		if (element.RemoveEventHandler)
		{
			return element.RemoveEventHandler(eventType, functionName, useCapture);
		}
		else if (element.removeEventListener)
		{
			element.removeEventListener(eventType, functionName, useCapture);
			return true;
		}
		else if (element.detachEvent)
		{
			return element.detachEvent('on' + eventType, functionName);
		}
	};
	this.GetPropertyList = function(obj, parent)
	{
		var msg = '', content;
		var prlist = new Array(), i = 0;
		for (var prop in obj)
		{
			if (++this.PropertyLimit > 50)
				break;
			content = typeof (obj[prop]);
			if (content == 'function')
			{
				prlist[i++] = [(parent ? parent + "." : "") + prop, 'function' + obj[prop].toString().match(/\(.*\)/)];
			}
			else if (content == 'object' && this.RecrsionLimit++ < 5)
			{
				prsub = this.GetPropertyList(obj[prop], (parent ? parent + "." : "") + prop);
				for (var j = 0; j < prsub.length; j++, i++)
					prlist.push(prsub[j]);
				this.RecrsionLimit--;
			}
			else
			{
				prlist[i++] = [(parent ? parent + "." : "") + prop, obj[prop]];
			}
		}
		return prlist;
	};
	this.RecrsionLimit = 0;
	this.IdentifyObject = function(obj, parent)
	{
		var str = '', list = Synthesys.FuncLib.GetPropertyList(obj, parent);
		for (var i = 0; i < list.length; i++)
			str += list[i][0] + '=' + list[i][1] + '\n';
		alert(str);
	};
	this.PrintIFrame = function(frameName)
	{
		var theFrame = frames[frameName];
		if (Synthesys.Browser.IsOpera)
		{
			var wind = window.open(theFrame);
			wind.opener.focus();
			wind.print();
			wind.close();
			wind = null;
		}
		theFrame.focus();
		if (Synthesys.Browser.IsIe && Synthesys.Browser.Version >= 7)
			theFrame.document.execCommand('print', true, null);
		else
			theFrame.print();
	};
	this.IsNumber = function(text, isFloat)
	{
		var testKey = /(^\d+$)/;
		if (isFloat)
			testKey = /(^\d+$)|(^\d+\.\d+$)/;
		if (testKey.test(text))
			return true;
		else
			return false;
	};
};

Synthesys.Browser = new function()
{
	this.UserAgent = navigator.userAgent.toLowerCase();
	this.CompatibleName = this.UserAgent.split('/')[0];
	this.CompatibleVersion = parseFloat(navigator.appVersion);
	this.IsIe = (this.UserAgent.indexOf('msie') == -1 ? false : true);
	this.IsFirefox = (this.UserAgent.indexOf('firefox') == -1 ? false : true);
	this.IsOpera = (this.UserAgent.StartsWith('opera') ? true : false);
	this.IsChrome = (this.UserAgent.indexOf('chrome') == -1 ? false : true);
	
	if (this.IsIe)
	{
		this.Name = 'Internet Explorer';
		this.Version = parseFloat(this.UserAgent.split('msie')[1]);
	}
	else if (this.IsFirefox)
	{
		this.Name = 'FireFox';
		this.Version = parseFloat(this.UserAgent.split('firefox/')[1]);
	}
	else if (this.IsChrome)
	{
		this.Name = 'Chrome';
		this.Version = parseFloat(this.UserAgent.split('chrome/')[1].split(' ')[0]);
	}
	else
	{
		this.Name = this.compatibleName;
		this.Version = this.compatibleVersion;
	}
	
	this.IsPageLoaded = false;
	Synthesys.FuncLib.AddLoadEvent(function() { Synthesys.Browser.IsPageLoaded = true; });
	
	this.GetScrollPosition = function()
	{
		var scrollp = new Synthesys.Point();
		if (typeof (window.pageYOffset) == 'number')
		{
			scrollp.Y = window.pageYOffset;
			scrollp.X = window.pageXOffset;
		}
		else if (document.body && (document.body.scrollLeft || document.body.scrollTop))
		{
			scrollp.Y = document.body.scrollTop;
			scrollp.X = document.body.scrollLeft;
		}
		else if (document.documentElement && (document.documentElement.scrollLeft ||
			document.documentElement.scrollTop))
		{
			scrollp.Y = document.documentElement.scrollTop;
			scrollp.X = document.documentElement.scrollLeft;
		}
		return scrollp;
	};
	this.CheckScrollbars = function()
	{
		var res = new Object();
		res.ScrollbarX = false;
		res.ScrollbarY = false;
		var scroll = this.GetScrollPosition();
		if (scroll.X > 0)
		{
			res.ScrollbarX = true;
		}
		else
		{
			document.body.scrollLeft = 1;
			scroll = this.GetScrollPosition();
			if (scroll.X > 0)
				res.ScrollbarX = true;
			document.body.scrollLeft = 0;
		}
		if (scroll.Y > 0)
		{
			res.ScrollbarY = true;
		}
		else
		{
			document.body.scrollTop = 1;
			scroll = this.GetScrollPosition();
			if (scroll.Y > 0)
				res.ScrollbarY = true;
			document.body.scrollTop = 0;
		}
		return res;
	};
	this.GetViewport = function()
	{
		var viewport = new function()
		{
			this.Width = 0;
			this.Height = 0;
			this.MinHeight = 0;
			this.MinWidth = 0;
			this.MaxHeight = 0;
			this.MaxWidth = 0;
		};
		if (window.innerWidth)
		{
			viewport.Width = window.innerWidth;
			viewport.Height = window.innerHeight;
		}
		else
		{
			var divBaba = document.getElementById('browser_viewport_dimen');
			if (!divBaba)
			{
				divBaba = document.createElement('DIV');
				divBaba.id = 'browser_viewport_dimen';
				divBaba.style.position = 'absolute';
				divBaba.style.height = '1px';
				divBaba.style.fontSize = '1px';
				divBaba.style.width = '1px';
				divBaba.style.right = '1px';
				divBaba.style.bottom = '12px';
				divBaba.style.zIndex = '-1';
				divBaba.style.background = 'white';
				document.body.appendChild(divBaba);
			}
			viewport.Width = divBaba.offsetLeft + divBaba.offsetWidth + 1;
			viewport.Height = divBaba.offsetTop + divBaba.offsetHeight + 12;
		}
		viewport.MaxHeight = viewport.Height;
		viewport.MaxWidth = viewport.Width;
		if (Synthesys.Browser.IsIe && Synthesys.Browser.Version < 7)
		{
			var scroll = this.GetScrollPosition();
			viewport.Width -= scroll.X;
			viewport.Height -= scroll.Y;
		}
		viewport.MinHeight = viewport.Height;
		viewport.MinWidth = viewport.Width;
		var bodyElem = document.body || document.documentElement;
		if (viewport.MaxHeight < bodyElem.clientHeight)
			viewport.MaxHeight = bodyElem.clientHeight;
		if (viewport.MaxWidth < bodyElem.clientWidth)
			viewport.MaxWidth = bodyElem.clientWidth;
		if (viewport.MinHeight > bodyElem.clientHeight)
			viewport.MinHeight = bodyElem.clientHeight;
		if (viewport.MinWidth > bodyElem.clientWidth)
			viewport.MinWidth = bodyElem.clientWidth;
		if (!Synthesys.Browser.IsIe)
		{
			if (viewport.Width < viewport.MaxWidth)
				viewport.Width -= 14;
			if (viewport.Height < viewport.MaxHeight)
				viewport.Height -= 14;
		}
		return viewport;
	};
};

Synthesys.QueryString = new function()
{
	var keyValuePairs = document.location.search.RemoveAt(0).split('&');
	this.GetKey = function(index)
	{
		return keyValuePairs[index].split('=')[0];
	};
	this.GetValue = function(key)
	{
		if (typeof (key) == 'number')
		{
			if (key > -1 && key < keyValuePairs.length)
				return keyValuePairs[key].split('=')[1];
			else
				return "";
		}
		else
		{
			key = key.toLowerCase();
			for (i = 0; i < keyValuePairs.length; i++)
				if (keyValuePairs[i].split('=')[0].toLowerCase() == key)
					return keyValuePairs[i].split('=')[1];
			return "";
		}
	};
	this.ToString = function()
	{
		return keyValuePairs.toString();
	};
};
