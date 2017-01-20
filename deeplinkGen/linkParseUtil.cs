using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace deeplinkUtil
{
    public class linkParseUtil
    {
        printUtil _pr = new printUtil();
        mapLinkData _mld = new mapLinkData();
        public mapLinkData mapParse()
        {
            bool isData = false;
            string thisMapRAW = "";
            string[] mapRAWarray = { };
            List<string> mapRAWdata = new List<string>();
            _mld.linkClass = getLinkClass("map");
            if(_mld.linkClass.ToLower() == "abort")
            {
                return null;
            }
            _pr.write(String.Format("{0}Get the map link for your desired location.", _pr._br), _pr._wht);
            _pr.write(String.Format("{0}Tip: Retrieve link using Google maps and the *share* button in order to get the proper format.", _pr._br), _pr._ylw);
            thisMapRAW = _pr.rl(String.Format("{0}Enter Link: ", _pr._br), _pr._drkGray, _pr._mgnta).Trim();
            if(thisMapRAW == null || thisMapRAW.Length < 1)
            {
                _pr.write(String.Format("{0}It Appears you have not entered in a proper URL.{0} You will be return the the Map Link Utility.", _pr._br), _pr._wht);
                _pr.rest(2000);
                _pr.resetConsole();
                return mapParse();
            }
            _mld.linkRAW = thisMapRAW;
            mapRAWarray = thisMapRAW.Split('/');
            foreach (string RAWdata in mapRAWarray)
            {
                if (!isData)
                {
                    if (RAWdata.ToLower() == "place" || RAWdata.ToLower() == "search")
                    {
                        isData = true;
                    }
                }
                else
                {
                    mapRAWdata.Add(RAWdata);
                }
            }
            if (mapRAWdata.Count() >= 2)
            {
                _mld.locationLong = mapRAWdata[0].Replace('+', ' ');
                _pr.write(String.Format("{0}Is " + _mld.locationLong + " the location you expected?", _pr._br), _pr._wht);
                ConsoleKeyInfo correctName = _pr.rk("(Y/N): ", _pr._drkGray, _pr._mgnta);
                if (correctName.KeyChar == 'y' || correctName.KeyChar == 'Y')
                {
                    _mld.linkTitle = getLinkTitle("Maps", _mld.locationLong);
                    string RAWlatlong = "";
                    string[] latlongArr = { };
                    RAWlatlong = mapRAWdata[1].TrimStart('@');
                    latlongArr = RAWlatlong.Split(',');
                    for (int i = 0; i <= latlongArr.Count() - 1; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                _mld.locLat = latlongArr[i];
                                break;
                            case 1:
                                _mld.locLon = latlongArr[i];
                                break;
                            case 2:
                                _mld.locZ = latlongArr[i].TrimEnd('z');
                                break;
                            default:
                                break;
                        }
                    }
                    _mld.ANDRDgoogMapLink = _mld.ANDRDMAPscheme + _mld.locLat + "," + _mld.locLon + "?q=" + _mld.locationLong.Replace(' ', '+');
                    _mld = getANDRDmapScript(_mld);
                    _mld.APPLmapLink = _mld.APPLMAPscheme + "?q=" + _mld.locationLong.Replace(' ', '+') + "&sll=" + _mld.locLat + ',' + _mld.locLon + "&z=" + _mld.locZ;
                    _mld = getAPPLmapScript(_mld);
                    _mld = getUNIVRSLmapScript(_mld);

                    return _mld;
                }
                else
                {
                    _pr.write(String.Format("{0}If the location is incorrect, you have likely entered an invalid URL.", _pr._br), _pr._red);
                    _pr.write(String.Format("{0}Try searching for the location again and be sure to retrieve the URL using the \"SHARE\" button.", _pr._br), _pr._gray);
                    _pr.write(String.Format("{0}You will now be returned to the previous menu. Press any key to proceed.", _pr._br), _pr._ylw);
                    _pr.proceed();
                    _pr.resetConsole();
                    return mapParse();
                }
            }
            else
            {
                _pr.write(String.Format("{0}You have likely entered an invalid URL.", _pr._br), _pr._red);
                _pr.write(String.Format("{0}Try searching for the location again and be sure to retrieve the URL using the \"SHARE\" button.", _pr._br), _pr._gray);
                _pr.write(String.Format("{0}You will now be returned to the previous menu. Press any key to proceed.", _pr._br), _pr._ylw);
                _pr.proceed();
                _pr.resetConsole();
                return mapParse();
            }
        }
        public stdLinkData universalLinkParse(stdLinkData _ld)
        {
            _ld.linkClass = getLinkClass(_ld.linkType);
            _ld.linkRAW = getLinkLocation(_ld.linkType);
            //get formatted links for android and apple for specific soc. media
            switch (_ld.linkType)
            {
                case "Twitter":
                    _ld.linkName = _ld.linkRAW.Split('/')[3];
                    _ld.ANDRDurl = _ld.ANDRDscheme + _ld.linkName;
                    _ld.APPLurl = _ld.APPLscheme + _ld.linkName;
                    _ld.linkTitle = getLinkTitle(_ld.linkType, _ld.linkName);
                    break;
                case "Instagram":
                    string[] andrdScheme = _ld.ANDRDscheme.Split('|');
                    _ld.ANDRDurl = $@"{andrdScheme[0]}{_ld.linkName}{andrdScheme[1]}";
                    _ld.APPLurl = $@"{_ld.APPLscheme}{_ld.linkName}";
                    _ld.linkTitle = getLinkTitle(_ld.linkType, _ld.linkName);
                    break;
                case "Google+":
                    string[] linkBits = _ld.linkRAW.Split('/');
                    for (int i = 0; i < linkBits.Count(); i++) { if (i > 1) { _ld.linkLONG += linkBits[i] + '/'; } }
                    _ld.ANDRDurl = _ld.ANDRDscheme + _ld.linkLONG;
                    _ld.APPLurl = _ld.APPLscheme + _ld.linkLONG;
                    _ld.linkTitle = getLinkTitle(_ld.linkType, _ld.linkName);
                    break;
                case "Facebook":
                    _ld = facebookParse(_ld);
                    break;
                default:
                    return null;
            }
            //insert links to scripts
            _ld = getDROIDscript(_ld);
            _ld = getAPPLscript(_ld);
            _ld = getUNIVRSLscript(_ld);

            return _ld;
        }
        public stdLinkData facebookParse(stdLinkData data)
        {
            List<string> cleanPageSource = new List<string>();
            string[] pageSource = { };
            WebClient myWebClient = new WebClient();
            myWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"); ///Needed to render certain sites that render based on useragent type
            byte[] myDataBuffer = myWebClient.DownloadData(data.linkRAW);
            string download = Encoding.ASCII.GetString(myDataBuffer);
            pageSource = download.Split('<');
            foreach(string item in pageSource)
            {
                cleanPageSource.Add("<" + item);
            }
            foreach(string item in cleanPageSource)
            {
                if (item.Contains("meta"))
                {
                    string[] metaString = item.Split(' ');
                    foreach(string _item in metaString)
                    {
                        if (_item.Contains("al:ios:url"))
                        {
                            foreach(string content in metaString)
                            {
                                if (content.Contains("content"))
                                {
                                    if (content.Contains("profile"))
                                    {
                                        string fbScheme = content.Split('=').Last();
                                        fbScheme = fbScheme.Replace(@"\", "");
                                        data.FbPageSchemeLink = fbScheme.Replace("\"", "");
                                    }
                                    else if (content.Contains("page"))
                                    {
                                        string _fbScheme = content.Split('=').Last();
                                        string fbScheme = "fb://page/" + _fbScheme;
                                        fbScheme = fbScheme.Replace(@"\", "").Replace("\"", "");
                                        data.FbPageSchemeLink = fbScheme;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            data.linkName = data.linkRAW.Split('/')[3];
            data.ANDRDurl = data.FbPageSchemeLink;
            data.APPLurl = data.FbPageSchemeLink;
            data.linkTitle = getLinkTitle(data.linkType, data.linkName);
            return data;
        }
        public string getLinkClass(string linkType)
        {
            _pr.write(String.Format("{0}Please enter the class of the \'a\' tag that this link relates to.{0}It must be specific to the {1} link.", _pr._br, linkType), _pr._wht);
            string linkClass = _pr.rl("Class: ", _pr._drkGray, _pr._mgnta);
            if (String.IsNullOrEmpty(linkClass) || String.IsNullOrWhiteSpace(linkClass))
            {
                getLinkClass(linkType);
            }
            return linkClass;
        }
        public string getLinkLocation(string locationType)
        {
            string linkRAW = _pr.rl("Please enter the " + locationType + " location you want to make into a deep link. (currently only supports profile pages)" + _pr._br + "URL: ", _pr._gray, _pr._mgnta);
            if (String.IsNullOrEmpty(linkRAW) || String.IsNullOrWhiteSpace(linkRAW))
            {
                getLinkLocation(locationType);
            }
            return linkRAW;
        }
        public string getLinkID(string locationType)
        {
            if(locationType == "Facebook")
            {
                _pr.write(_pr._br + "On the following page, enter the facebook pages url, then copy the ID number on the following page to be used in the next step.", _pr._mgnta);
                _pr.rk(_pr._br + "Press any key to continue: ", _pr._gray, _pr._mgnta);
                Process.Start("http://findmyfbid.com/");
                string idNumber =_pr.rl(_pr._br + "Enter given ID number: ", _pr._gray, _pr._mgnta);
                char[] numbers = idNumber.ToCharArray();
                string checkedNumber = "";
                foreach(char c in numbers)
                {
                    if (c < '0' || c > '9')
                    {
                        _pr.write(_pr._br + "The ID number you supplied contained a character other than a number." + _pr._br + "You will be prompted to re-enter the ID number.", _pr._red);
                        return getLinkID(locationType);
                    }
                    else
                    {
                        checkedNumber += c;
                    }
                }
                return checkedNumber;
            }
            else
            {
                return "0";
            }
        }
        public int getUNIVRSLint(string message)
        {
            int intReturn = 0;
            bool result = int.TryParse(_pr.rl(_pr._br + message + ": ", _pr._drkGray, _pr._mgnta), out intReturn);
            if (result)
            {
                return intReturn;
            }
            return getUNIVRSLint(message);
        }
        public string getLinkTitle(string linkType, string linkName)
        {
            _pr.write(String.Format("{1}Create a title for your {0} link.{1}", linkType, _pr._br), _pr._wht);
            if(linkType != "Google+")
            {
                _pr.write("Enter a blank line ", _pr._mgnta);
                _pr.write(String.Format("for default Title: {1} on {2}!{0}Or", _pr._br, linkName, linkType), _pr._wht);
            }
            _pr.write(" Enter a custom title (greater than 2 characters)", _pr._mgnta);
            _pr.write(" to fit in the format: (Your title) on " + linkType + "!", _pr._wht);
            string correctTitle = _pr.rl(_pr._br + "Title: ", _pr._drkGray, _pr._mgnta).Trim();
            if (String.IsNullOrEmpty(correctTitle) && linkType != "Google+")
            {
                return linkName + " on " + linkType + "!";
            }
            else if (String.IsNullOrEmpty(correctTitle) && linkType == "Google+")
            {
                _pr.write(_pr._br + "The title must be greater than 2 characters in length.", _pr._gray);
                _pr.rest(2000);
                return getLinkTitle(linkType, linkName);
            }
            else if (correctTitle.Length > 2)
            {
                string action = _pr.rl("Is \"" + correctTitle + " on " + linkType + "!\" an acceptable link title?", _pr._drkGray, _pr._mgnta);
                action = action.ToLower();
                if(action == "y" || action == "yes")
                {
                    return correctTitle + " on " + linkType + "!";
                }
                else
                {
                    _pr.write("Ok, you'll be returned to the Title menu.", _pr._wht);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    return getLinkTitle(linkType, linkName);
                }
            }
            else
            {
                _pr.write(_pr._br + "The title must be greater than 2 characters in length.", _pr._gray);
                _pr.rest(2000);
                return getLinkTitle(linkType, linkName);
            }
        }
        public stdLinkData getUNIVRSLscript(stdLinkData data)
        {
            data.UNIVRSLscript = $@"
        //Universal {data.linkType} Script
        $('.{data.linkClass}').attr('title','{data.linkTitle}');
";
            return data;
        }
        public mapLinkData getUNIVRSLmapScript(mapLinkData data)
        {
            data.UNVRSLmapScript = $@"
        //Universal Map Script
        $('.{data.linkClass}').attr('title','{data.linkTitle}');
";
            return data;
        }
        public stdLinkData getDROIDscript(stdLinkData data)
        {
            data.ANDRDscript = $@"
        //Android {data.linkType} script
        $('.{data.linkClass}').attr('href', '{data.ANDRDurl}');
        $('.{data.linkClass}').attr('target', '_self');    
";
            return data;
        }
        public mapLinkData getANDRDmapScript(mapLinkData data)
        {
            data.ANDRDscript = $@"
        //Android Maps script
        $('.{data.linkClass}').attr('href', 'geo:{data.locLat},{data.locLon}?q={data.locationLong}&z={ data.locZ}');
        $('.{data.linkClass}').attr('target', '_self');
            ";
            return data;
        }
        public stdLinkData getAPPLscript(stdLinkData data)
        {
            data.APPLscript = $@"
        //Apple {data.linkType} script
        $('.{data.linkClass}').attr('href', '{data.APPLurl}');
        $('.{data.linkClass}').attr('target', '_self');
        $('.{data.linkClass}').click(function(){{
            var clickedAt = +new Date;
            if(clickedAt - +new Date < 500){{
                window.open('{data.linkRAW}');
            }}
        }});
";
            return data;
        }
        public mapLinkData getAPPLmapScript(mapLinkData data)
        {
            data.APPLscript = $@"
        //Apple Maps script
        $('.{data.linkClass}').attr('href', 'http://maps.apple.com/?q={data.locationLong}&sll={data.locLat},{data.locLon}&z={data.locZ}');
        $('.{data.linkClass}').attr('target', '_self');
            ";
            return data;
        }
    }
}
