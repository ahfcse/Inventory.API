namespace Inventory.API.Exceptions
{
    public class DuplicateBarcodeException : Exception
    {
        /// <summary>
        /// The duplicate barcode that caused the exception
        /// </summary>
        public string Barcode { get; }

        /// <summary>
        /// Creates a new instance of DuplicateBarcodeException
        /// </summary>
        /// <param name="barcode">The duplicate barcode</param>
        public DuplicateBarcodeException(string barcode)
            : base($"Barcode '{barcode}' already exists in the system. Barcodes must be unique.")
        {
            Barcode = barcode;
        }

        /// <summary>
        /// Creates a new instance of DuplicateBarcodeException with custom message
        /// </summary>
        /// <param name="barcode">The duplicate barcode</param>
        /// <param name="message">Custom error message</param>
        public DuplicateBarcodeException(string barcode, string message)
            : base(message)
        {
            Barcode = barcode;
        }

        /// <summary>
        /// Creates a new instance of DuplicateBarcodeException with inner exception
        /// </summary>
        /// <param name="barcode">The duplicate barcode</param>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public DuplicateBarcodeException(string barcode, string message, Exception innerException)
            : base(message, innerException)
        {
            Barcode = barcode;
        }
    }
}
