using System;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf.Canvas;
using iText.Forms.Xfdf;

class AddTimerInPdf
{
    static void Main(string[] args)
    {
        string inputPdfPath = @"path\to\your\existing.pdf";  // Path to the existing PDF
        string outputPdfPath = @"path\to\your\output.pdf";  // Path to save the new PDF with form fields

        // Open the existing PDF
        using (PdfReader reader = new PdfReader(inputPdfPath))
        using (PdfWriter writer = new PdfWriter(outputPdfPath))
        {
            PdfDocument pdfDoc = new PdfDocument(reader, writer);

            // Create an AcroForm (form) instance
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            // Add a text field for the question: "What is your name?"
            AddTextField(form, pdfDoc, "NameField", 100, 600, 200, 20, "Enter your name");

            // Add a checkbox for the question: "Do you like programming?"
            AddCheckBoxField(form, pdfDoc, "ProgrammingCheckbox", 100, 550, "Yes");

            // Add a timer field for duration
            AddTimerField(form, pdfDoc, "DurationField", 100, 500, 200, 20, "Duration: 0");

            // Add JavaScript to start and stop the timer
            string js = @"
                var startTime = new Date().getTime();  // Record start time when the PDF is opened

                this.setTimeField = function() {
                    var endTime = new Date().getTime();  // Record end time when PDF is closed or submitted
                    var duration = (endTime - startTime) / 1000;  // Duration in seconds
                    this.getField('DurationField').value = 'Duration: ' + duration + ' seconds';
                };

                // Call setTimeField function when the document is closed
                this.addEventListener('WillClose', this.setTimeField);";

            // Add the JavaScript action to the document
            PdfAction action = PdfAction.CreateJavaScript(js);
            pdfDoc.GetCatalog().SetOpenAction(action);

            // Close the document
            pdfDoc.Close();

            Console.WriteLine("PDF modified with interactive form fields and timer!");
        }
    }

    // Function to add a text field (for user input)
    static void AddTextField(PdfAcroForm form, PdfDocument pdfDoc, string fieldName, float x, float y, float width, float height, string defaultValue)
    {
        // Create a text field for user input
        PdfTextFormField textField = PdfFormField.CreateText(pdfDoc, new iText.Kernel.Geom.Rectangle(x, y, width, height), fieldName, defaultValue);
        
        // Add the field to the form
        form.AddField(textField);
    }

    // Function to add a checkbox (for Yes/No answers)
    static void AddCheckBoxField(PdfAcroForm form, PdfDocument pdfDoc, string fieldName, float x, float y, string defaultValue)
    {
        // Create a checkbox form field
        PdfCheckBoxFormField checkBox = PdfFormField.CreateCheckBox(pdfDoc, new iText.Kernel.Geom.Rectangle(x, y, 20, 20), fieldName);

        // Set the default value (checked or unchecked)
        checkBox.SetCheckType(PdfCheckBoxFormField.TYPE_CHECK);
        checkBox.SetValue(defaultValue);

        // Add the checkbox to the form
        form.AddField(checkBox);
    }

    // Function to add a field that will display the duration of time
    static void AddTimerField(PdfAcroForm form, PdfDocument pdfDoc, string fieldName, float x, float y, float width, float height, string defaultValue)
    {
        // Create a text field for the timer (duration)
        PdfTextFormField timerField = PdfFormField.CreateText(pdfDoc, new iText.Kernel.Geom.Rectangle(x, y, width, height), fieldName, defaultValue);
        
        // Set it as read-only since it's just for display
        timerField.SetReadOnly(true);

        // Add the field to the form
        form.AddField(timerField);
    }
}
