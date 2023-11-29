# Canadian Currency Converter

Canadian Currency Converter is a console application to convert currency values from/to Canadian Dollars.

The application consumes the [Bank of Canada Valet API](https://www.bankofcanada.ca/valet/docs) to get the daily exchange values.

## Installation

* Clone the repository 
* Open the solution file CurrencyConverter.sln
* Start

## Usage


```console
--- Welcome to the Canadian Currency Converter ---

What currency do you want to convert?
1 - USD
2 - EUR
3 - JPY
4 - GBP
5 - AUD
6 - CHF
7 - CNY
8 - HKD
9 - MXN
10 - INR
Type the selected option number:

Do you want to convert TO Canadian dollars or FROM Canadian dollars?
1 - USD to CAD
2 - From CAD to USD
Type the selected option number:

How much do you want to convert?
Type the value:

Do you want to specify an exchange date? If not, the most recent published rate will be used.(Bank of Canada publishes daily rates once each business day by 16:30 ET.)
1 - Yes
2 - No
Type the selected option number:
Type the exchange date (format: MM/DD/YYYY):

--- Conversion Results ---
Original Value: 1.0000 USD
Converted Value: 1.3635 CAD
Exchange Date: 11/27/2023
Exchange Rate: 1.3635
--------------------------
```

