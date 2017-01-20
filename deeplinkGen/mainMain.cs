using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

    namespace deeplinkUtil
{
    public class mainMain
    {
        printUtil _pr = new printUtil();
        linkParseUtil _lp = new linkParseUtil();
        mapLinkData _mld = new mapLinkData();
        stdLinkData data = new stdLinkData();
        URLSchemes scheme = new URLSchemes();
        scriptPack scripts = new scriptPack();
        writer createScripts = new writer();
        /// GLOBAL VARS
        public string baseFilepath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        public void menuInit()
        {
            ///Any actions that need to be done before program start
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetWindowSize(100, Console.WindowHeight);

            menu();
        }
        public void menu()
        {
            
            _pr.write(_pr._br + "Welcome to the deeplink generator for mobile devices.", _pr._gray);
            _pr.write(_pr._br + "Please select one of the following options.", _pr._mgnta);
            _pr.write(_pr._br + "1)", _pr._mgnta);
            _pr.write(" Create new deeplink for one or more applications and links.", _pr._gray);
            _pr.write(_pr._br + "x)", _pr._mgnta);
            _pr.write(" Exit Application.", _pr._gray);
            ConsoleKeyInfo selection = _pr.rk(_pr._br + "Enter Selection: ", _pr._drkGray, _pr._mgnta);
            switch (selection.KeyChar)
            {
                case '1':
                    buildLinkPack();
                    _pr.resetConsole();
                    menu();
                    break;
                case 'x':
                case 'X':
                    Environment.Exit(0);
                    break;
                default:
                    _pr.resetConsole();
                    menu();
                    break;
            }
        }
        public void buildLinkPack()
        {
            data = new stdLinkData();
            _pr.write(_pr._br + "We will go through links one by one to create deeplinks, supported links include...", _pr._gray);
            _pr.write(_pr._br + "1)", _pr._mgnta);
            _pr.write(" Maps", _pr._gray);
            _pr.write(_pr._br + "2)", _pr._mgnta);
            _pr.write(" Facebook", _pr._gray);
            _pr.write(_pr._br + "3)", _pr._mgnta);
            _pr.write(" Google+", _pr._gray);
            _pr.write(_pr._br + "4)", _pr._mgnta);
            _pr.write(" Twitter", _pr._gray);
            _pr.write(_pr._br + "5)", _pr._mgnta);
            _pr.write(" Instagram", _pr._gray);
            _pr.write(_pr._br + "6)", _pr._mgnta);
            _pr.write(" Write Scripts to File", _pr._gray);
            _pr.write(_pr._br + "x)", _pr._mgnta);
            _pr.write(" Abort Script creation", _pr._gray);
            ConsoleKeyInfo linkType = _pr.rk(_pr._br + "Enter Selection: ", _pr._drkGray, _pr._mgnta);
            Char _linkType = linkType.KeyChar;
            switch (linkType.KeyChar)
            {
                case '1':
                    _mld = _lp.mapParse();
                    if(_mld == null)
                    {
                        _pr.write(_pr._br + "You have aborted creating this link. Press any key to previous menu.", _pr._gray);
                        _pr.proceed();
                        _pr.resetConsole();
                        buildLinkPack();
                    }
                    scripts.APPLmapScript = _mld.APPLscript;
                    scripts.ANDRDmapScript = _mld.ANDRDscript;
                    scripts.UNVRSLmapScript = _mld.UNVRSLmapScript;
                    _pr.write(_pr._br + "Returning to previous menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    buildLinkPack();
                    break;
                case '2':
                    data.linkType = "Facebook";
                    data.UNIVRSLscheme = scheme.FBscheme;
                    data = _lp.universalLinkParse(data);
                    if(data == null)
                    {
                        break;
                    }
                    scripts.APPLfbScript = data.APPLscript;
                    scripts.ANDRDfbScript = data.ANDRDscript;
                    scripts.UNVRSLfbScript = data.UNIVRSLscript;
                    _pr.write(_pr._br + "Returning to previous menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    buildLinkPack();
                    break;
                case '3':
                    data.linkType = "Google+";
                    data.ANDRDscheme = scheme.GPscheme;
                    data.APPLscheme = scheme.GPscheme;
                    data = _lp.universalLinkParse(data);
                    if(data == null)
                    {
                        break;
                    }
                    scripts.APPLgoogPlusScript = data.APPLscript;
                    scripts.ANDRDgoogPlusScript = data.ANDRDscript;
                    scripts.UNVRSLgoogPlusScript = data.UNIVRSLscript;
                    _pr.write(_pr._br + "Returning to previous menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    buildLinkPack();
                    break;
                case '4':
                    data.linkType = "Twitter";
                    data.ANDRDscheme = scheme.TWTRscheme;
                    data.APPLscheme = scheme.TWTRscheme;
                    data = _lp.universalLinkParse(data);
                    if (data == null)
                    {
                        break;
                    }
                    scripts.APPLtwitterScript = data.APPLscript;
                    scripts.ANDRDtwitterScript = data.ANDRDscript;
                    scripts.UNVRSLtwitterScript = data.UNIVRSLscript;
                    _pr.write(_pr._br + "Returning to previous menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    buildLinkPack();
                    break;
                case '5':
                    data.linkType = "Instagram";
                    data.ANDRDscheme = scheme.IgANDRDscheme;
                    data.APPLscheme = scheme.IgAPPLscheme;
                    data = _lp.universalLinkParse(data);
                    if (data == null)
                    {
                        break;
                    }
                    scripts.APPLinstagramScript = data.APPLscript;
                    scripts.ANDRDinstagramScript = data.ANDRDscript;
                    scripts.UNVRSLinstagramScript = data.UNIVRSLscript;
                    _pr.write(_pr._br + "Returning to previous menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    buildLinkPack();
                    break;
                case '6':
                    List<string> rawScripts = new List<string>();
                    rawScripts = scriptList(scripts);
                    string formattedScript = $@"$(document).ready(function(){{
    {rawScripts[0]}    
    var ua = navigator.userAgent.toLowerCase();
    if(ua.search('iphone') != -1 || ua.search('ipod') != -1 ||ua.search('ipad') != -1){{
         {rawScripts[1]}
    }}
    else if(ua.search('android') != -1){{
        {rawScripts[2]}
    }}
}}); ";
                    createScripts.writeScripts(formattedScript, baseFilepath);
                    _pr.write(_pr._br + "The script for your maps app has been written. ", _pr._grn);
                    _pr.write(_pr._br + "Returning to Main Menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    break;
                case 'x':
                case 'X':
                    _pr.write(_pr._br + "Returning to Main Menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    break;
                default:
                    _pr.write(_pr._br + "Invalid entry. Returning to Main Menu.", _pr._gray);
                    _pr.rest(2000);
                    _pr.resetConsole();
                    break;
            }
        }
        public List<string> scriptList(scriptPack scriptList)
        {
            List<string> newScripts = new List<string>();
            string applScript = " ";
            string andrdScript = " ";
            string universalScript = " ";
            ///universal script
            universalScript = addScriptString(scriptList.UNVRSLfbScript, universalScript);
            universalScript = addScriptString(scriptList.UNVRSLgoogPlusScript, universalScript);
            universalScript = addScriptString(scriptList.UNVRSLinstagramScript, universalScript);
            universalScript = addScriptString(scriptList.UNVRSLmapScript, universalScript);
            universalScript = addScriptString(scriptList.UNVRSLtwitterScript, universalScript);
            ///apple script
            applScript = addScriptString(scriptList.APPLfbScript, applScript);
            applScript = addScriptString(scriptList.APPLgoogPlusScript, applScript);
            applScript = addScriptString(scriptList.APPLinstagramScript, applScript);
            applScript = addScriptString(scriptList.APPLmapScript, applScript);
            applScript = addScriptString(scriptList.APPLtwitterScript, applScript);
            ///android script
            andrdScript = addScriptString(scriptList.ANDRDfbScript, andrdScript);
            andrdScript = addScriptString(scriptList.ANDRDgoogPlusScript, andrdScript);
            andrdScript = addScriptString(scriptList.ANDRDinstagramScript, andrdScript);
            andrdScript = addScriptString(scriptList.ANDRDmapScript, andrdScript);
            andrdScript = addScriptString(scriptList.ANDRDtwitterScript, andrdScript);

            newScripts.Add(universalScript);
            newScripts.Add(applScript);
            newScripts.Add(andrdScript);
            return newScripts;
        }
        public string addScriptString(string scriptPack, string scriptRAW)
        {
            if(scriptRAW != null)
            {
                scriptPack += scriptRAW;
            }
            return scriptPack;
        }
    }
}
