<?php
@ $path = GetSecurePath($_GET['page']);

function GetSecurePath($file="")
{
	$file = str_replace ("..","",$file);
	$path = $file;

	if ("index.php" == $path or "" == $path)
	{
		$path = $base . 'default.html';
	}
	return $path;
}
?>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html>
<head>
<title>NetReflector: A Dynamic Reflective Popluator for .NET</title>
<meta name="author" content="R. Owen Rogers">
<meta name="description" content="NetReflector: A Dynamic Reflective Popluator for .NET">
<meta name="KEYWORDS" content="NetReflector dynamic reflective popluator .NET C#">
<style type="text/css" media="all">@import "style.css";</style>
<link rel="alternate style sheet" media="print" type="text/css" href="print.css" title="Printable" />
</head>
<body>

<div id="header">
	<a href="http://netreflector.sourceforge.net/"><img src="images/netreflector.header.gif" border="0" width="306" height="62"></a>

	<div id="menu">
		<a href="?page=default.html" title="NetReflector home page">Home</a> |
		<a href="http://sourceforge.net/projects/netreflector/" title="NetReflector SourceForge project page">SourceForge</a> |
		<a href="?page=download.html">Download</a> |
		<a href="?page=tutorial.html">Tutorial</a> |
		<a href="?page=cvs.html" title="Tutorial on getting set up with CVS">CVS</a>
	</div>
</div>

<div id="content">
<?php
if (file_exists ($path)) {
	require_once("$path");
}
else {
	print "<p>Could not find page: $path</p>";
	return;
}
print ('<p class="lastmod">Page updated: ' . date("Y-m-d", filemtime(addslashes($path))) . '</p>');
?>
</div>

<div id="sidebar">

<h3>Feedback</h3>
- <a href="?page=contributors.html" title="List of the contributors to the COMUnit project">COMUnit contributors</a><br />
- <a href="http://lists.sourceforge.net/lists/listinfo/comunit-users" title="Sign up to post questions and participate in discussions on COMUnit">COMUnit mailing list</a><br />
- <a href="http://sourceforge.net/survey/survey.php?group_id=26984&survey_id=12039" title="Please submit your comments and opinion of COMUnit">COMUnit Questionnaire</a><br />
- <a href="?page=website.html" title="Information about this web site">Web site Information</a><br />

<h3>Links</h3>
- <A href="http://www.xprogramming.com" title="Download source for all xUnits">www.xprogramming.com</A><br/>
- <a href="http://www.junit.org" title="Home of the original JUnit">www.junit.org</a><br/>
- <a href="http://aspunit.sourceforge.net" title="A unit testing framework for ASP, based on COMUnit">ASPUnit</a><br/>

<p>
	<A href="http://sourceforge.net"> <IMG src="http://sourceforge.net/sflogo.php?group_id=26984" width="88" height="31" border="0" alt="SourceForge Logo"></A>
</p>

</div>

</body>
</html>