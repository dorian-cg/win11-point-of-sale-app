# Point of Sale (POS) app for Windows 11
Point of Sale (POS) app for Windows 11 using a simple spreadsheet as database. It generates a spreadsheet for each "sale" with the list of products sold and their quantities.

## Screenshots

### Light Theme
<img src="https://raw.githubusercontent.com/dorian-cg/win11-point-of-sale-app/refs/heads/main/docs/screenshots/light-theme.png">

### Dark Theme
<img src="https://raw.githubusercontent.com/dorian-cg/win11-point-of-sale-app/refs/heads/main/docs/screenshots/dark-theme.png">

## Data Source

### Example DataSet (used in previous screenshots)
<img src="https://raw.githubusercontent.com/dorian-cg/win11-point-of-sale-app/refs/heads/main/docs/screenshots/data-example.png">

### Download Example DataSet file
<a href="https://raw.githubusercontent.com/dorian-cg/win11-point-of-sale-app/refs/heads/main/docs/sample_db_file/MINIPOS_DATA.xlsx" target="_blank">
Download MINIPOS_DATA.xlsx
</a>

## Configuring Data Source
By default the app will attempt to find a file called `MINIPOS_DATA.xlsx` in the `Desktop` folder of the user.

| ENV VAR                  | Default value                    | Purpose                                                                      |
|--------------------------|----------------------------------|------------------------------------------------------------------------------|
| `MINIPOS_DEFAULT_FOLDER` | User's desktop folder path       | Folder used to output generated `.xlsx` files when finish button is clicked. |
| `MINIPOS_SRC_FILEPATH`   | Same as `MINIPOS_DEFAULT_FOLDER` | Folder that contains the data source `.xlsx` file.                           |
| `MINIPOS_SRC_FILENAME`   | `MINIPOS_DATA.xlsx`              | Name of `.xlsx` source file, must include extension.                         |

### Data Source Format

The data source must have 3 columns, the name of the columns is not important, but the order:

| ID       | Product Name | Product Price |
|----------|--------------|---------------|
| 1        | Milk         | 2.00          |
| 2        | Bread        | 3.99          |
| 3        | Chips        | 1.50          |

> **IMPORTANT**: Prices must use `,` as `thousands` separator and `.` for `decimals` separator. Prices must **not** contain `currency` symbols.

# Author's Note
Feel free to use this Open-Source sample project as the base for your custom version of a POS app. I'm aware it won't likely be mature enough for daily use because it's too generic, but it can be taken as start point to  improve and adapt as needed.