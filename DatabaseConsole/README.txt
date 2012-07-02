READ ME for Database Console

The Database Console is an application for managing the suite's database containing stock price history.  This app will allow the user to import ticker symbols into the database, and to retrieve price history for ticker symbols from Google.com.

LICENSE

This project is licensed under the GPL 3.0.

BUILDING THE DATABASE

I'm using SQL Server 2008 Express.  To re-create the database, create a database titled StockScreener and run the SQL script contained in BuildDatabase.sql.

POPULATING THE DATABASE

1) Populate the database with stock ticker symbols by selecting "Manage Stock Tickers -> Repopulate Ticker List in Database".  (Note:  This will import the ticker symbols from the CSV files contained in the TickerLists directory.  These CSV files were obtained from http://www.nasdaq.com/screening/company-list.aspx.  Current versions of these files can be obtained from that website.)

2) Download price history from Google by selecting "Manage Stock Tickers -> Update Price History for All Tickers".

WATCH LISTS

This app will allow you to create, view, and delete watch lists of ticker symbols.  These watch lists provide a means for updating the price histories of only certain stocks, rather than downloading data from all stocks.  Currently, the back tester application is the only means for adding symbols to a watch list. (Symbols that pass a back test will be added to a list.)