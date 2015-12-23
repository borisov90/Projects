<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnderConstruction.aspx.cs"
    Inherits="ETEM.Share.UnderConstruction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Site-Print.css" rel="stylesheet" type="text/css" media="print" />
    
    <meta charset="UTF-8" name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" type="text/css" href="../css/default.css" />
    <link rel="stylesheet" type="text/css" href="../css/component.css" />
    <link href="../Styles/mediaQueries.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap/bootstrap.css" rel="stylesheet" type="text/css" />
    <%--to test--%>
    <link href="../css/ui-lightness/jquery-ui-1.10.4.custom.css" rel="stylesheet" type="text/css" />
    <%--to test--%>
    <link href="../css/foundation-icons/foundation-icons.css" rel="stylesheet" type="text/css" />
    <link href="../css/css3buttons/css3buttons.css" rel="stylesheet" type="text/css" />
    <script src="../js/modernizr.custom.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <%--to test--%>
    <script src="../Scripts/jquery-ui-1.10.4.custom.js" type="text/javascript"></script>
    <%--to test--%>
    <script src="../Scripts/bootsrap-js/bootstrap.js" type="text/javascript"></script>
    <%--    <script type="text/javascript" language="javascript" src="js/common-scripts.js"></script>--%>

    <style type="text/css">
        html
        {
            font-family: sans-serif;
        }
       
        
        .sb-toggle-left, .sb-toggle-right, .sb-open-left, .sb-open-right, .sb-close
        {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <div id="sb-site">
        <form id="form1" runat="server">
        <div class="span4">
            <header class="clearfix"><h1>  Системата временно е спряна. Моля, да ни извините</h1></header>
            <ul>
                <li class="sb-toggle-left">Toggle left Slidebar</li>
                <li class="sb-toggle-right">Toggle right Slidebar</li>
                <li class="sb-open-left">Open left Slidebar</li>
                <li class="sb-open-right">Open right Slidebar</li>
                <li class="sb-close">Close either Slidebar</li>
            </ul>
        </div>
        </form>
    </div>
    <div class="sb-slidebar sb-left sb-style-overlay" style="margin-right: -284px;margin-top:0px; -webkit-transform: translate(-284px);">
    <nav>
		<ul class="sb-menu">
			<li><img src="http://plugins.adchsm.me/slidebars/images/slidebars-logo-white@2x.png" alt="Slidebars" width="118" height="40" /></li>
			<li><a href="http://plugins.adchsm.me/slidebars/">Home</a></li>
			<li><a href="http://plugins.adchsm.me/slidebars/index.php#download">Download</a></li>
			<li><a href="http://plugins.adchsm.me/slidebars/usage.php">Usage</a></li>
			<li><a href="http://plugins.adchsm.me/slidebars/usage.php#api">API</a></li>
			<li><a href="http://plugins.adchsm.me/slidebars/compatibility.php">Compatibility</a></li>
			<li><a href="#" class="sb-toggle-submenu">Help &amp; Issues <span class="sb-caret"></span></a>
				<ul class="sb-submenu">
					<li><a href="http://plugins.adchsm.me/slidebars/issues.php">Overview</a></li>
					<li><a href="http://plugins.adchsm.me/slidebars/issues/fixed-position.php">Fixed Positions</a></li>
					<li><a href="http://plugins.adchsm.me/slidebars/issues/modal.php">Modal.js</a></li>
					<li><a href="http://plugins.adchsm.me/slidebars/issues/squashed-content.php">Squashed Navbar Content</a></li>
				</ul>
			</li>
			<li><a href="http://plugins.adchsm.me/slidebars/contact.php">Contact</a></li>
			<li><a class="github" href="https://github.com/adchsm/Slidebars">Github</a></li>
			<li><span class="sb-open-right">About the Author</span></li>
			<li><small>Slidebars © 2014 Adam Smith</small></li>
		</ul>
	</nav>
    </div>

    <div class="sb-slidebar sb-right sb-style-overlay" style="margin-right: -284px;margin-top:0px; -webkit-transform: translate(-284px);">
			test-test-right
		</div>
    <%--to test--%>
    <link href="../Scripts/slidebars/slidebars.min.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/slidebars/sliderbars-custom-styles.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/slidebars/slidebars.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                // Initiate Slidebars
                $.slidebars();

                // Slidebars Submenus
                $('.sb-toggle-submenu').off('click').on('click', function () {
                    $submenu = $(this).parent().children('.sb-submenu');
                    $(this).add($submenu).toggleClass('sb-submenu-active'); // Toggle active class.

                    if ($submenu.hasClass('sb-submenu-active')) {
                        $submenu.slideDown(200);
                    } else {
                        $submenu.slideUp(200);
                    }
                });
            });
        })(jQuery);
		</script>
  <%--  <script >
        (function ($) {
            $(document).ready(function () {
                $.slidebars();
            });
        })(jQuery);
		</script>--%>
    <%--to test--%>
</body>
</html>
