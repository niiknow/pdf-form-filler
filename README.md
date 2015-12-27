#PDF Form Filler

The PDF Form Filler is a C# and ASP.NET MVC (Visual Studio 2015) project to help with populating of PDF forms.

MVC Project
===========
1. Endpoint to GET field names.
2. Endpoint to GET JSON definition or to populate a Web Form.
3. Endpoint to POST flatten key value pair as content type of applicaiton/json or application/x-www-form-urlencoded

Console Project
===============
A console application that populate the form with json data.

FillPdfForm.exe theform.pdf formdata.json outfile.pdf
