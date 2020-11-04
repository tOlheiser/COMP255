// This method takes in a string and applies that to my error label when called
        public void PrintErrorMessage(string ErrorMessage)
        {
            ErrorLabel.Content = ErrorMessage;
        }

        // Display an error message & return true if an input is missing
        public bool IsInputMissing() {
            if (StartYearTextbox.Text == "" ||
                EndYearTextbox.Text == ""   ||
                MinSalesTextbox.Text == ""  ||
                MaxSalesTextbox.Text == ""  ||
                ExpenseRateTextbox.Text == "")
            {
                PrintErrorMessage("All inputs are required");
                return true;
            } else {
                return false;
            }
        }

        // If what's passed in is not a number, display a message and return true.
        public bool IsInvalidInput(int StartYear, int EndYear, int MinSales, int MaxSales, double ExpenseRate) {
            if (!Int32.TryParse(StartYearTextbox.Text, out StartYear)){
                PrintErrorMessage("Start Year is not a number");
                return true;
            }
            else if (!Int32.TryParse(EndYearTextbox.Text, out EndYear)){
                PrintErrorMessage("End Year is not a number");
                return true;
            }
            else if (!Int32.TryParse(MinSalesTextbox.Text, out MinSales)) {
                PrintErrorMessage("Min Sales is not a number");
                return true;
            }
            else if (!Int32.TryParse(MaxSalesTextbox.Text, out MaxSales)) {
                PrintErrorMessage("Max Sales is not a number");
                return true;
            }
            else if (!Double.TryParse(ExpenseRateTextbox.Text, out ExpenseRate)) {
                PrintErrorMessage("Expense Rate is not a number");
                return true;
            } else {
                return false;
            }
        }

        public bool IsInvalidYear(int YearStart, int YearEnd) {
            // YearEnd must be greater than YearStart
            if (YearEnd < YearStart) {
                // If it isn't, display an error message
                PrintErrorMessage("End Year must be greater than Start Year");
                return true;
            // The difference between YearEnd and YearStart cannot be greater than 25
            } else if (YearEnd - YearStart > 25) {
                PrintErrorMessage("The difference between the years cannot be greater than 25");
                return true;
            } else {
                return false;
            }
        }

        // Expense rate must fall between 0 and 100
        public bool IsInvalidExpenseRate(double ExpRate) {
            if (ExpRate < 0 || ExpRate > 100) {
                // Display a message if ExpRate falls outside of the acceptable range
                PrintErrorMessage("The Expense Rate must be between 0 and 100");
                return true;
            } else {
                return false;
            }
        }

        // Max Sales must be greater than min sales
        public bool IsInvalidSales(int MaxSales, int MinSales) {
            if (MaxSales < MinSales) {
                // Display a message if MaxSales is less than MinSales
                PrintErrorMessage("Maximum Sales must be greater than Minimum Sales");
                return true;
            } else {
                return false;
            }
        }

        public bool ValidateForm() {
            if (IsInputMissing() && 
                IsInvalidInput(StartYear, EndYear) && 
                IsInvalidYear() && 
                IsInvalidExpenseRate() && 
                IsInvalidSales())
        }