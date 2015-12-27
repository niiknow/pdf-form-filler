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

License
========

MIT - Copyright © 2015 niiknow <friends@niiknow.org> 

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.