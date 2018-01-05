import { autoinject } from 'aurelia-framework';
import { GridOptions, GridReadyEvent, ColumnApi, GridApi, IDatasource, IGetRowsParams } from "ag-grid";
import { HttpClient, json } from 'aurelia-fetch-client';
import { DataItem } from './dataItem';

@autoinject
export class List {
    private gridOptions: GridOptions;
    private http: HttpClient;
    private columnApi: ColumnApi;
    private api: GridApi;

    constructor(http: HttpClient) {
        this.http = http;
        this.http.configure(config => {
            config.useStandardConfiguration();
            config.withDefaults({
                headers: {
                    'Accept': 'application/json'
                }
            });
        });
        this.gridOptions = <GridOptions>{};
        this.gridOptions.enableFilter = true;
        this.gridOptions.enableServerSideSorting = true;
        this.gridOptions.rowModelType = 'infinite';
        this.gridOptions.paginationPageSize = 10;
        this.gridOptions.infiniteInitialRowCount = 1;
        this.gridOptions.maxBlocksInCache = 2;
        this.gridOptions.sortingOrder = ['desc', 'asc'];
        this.gridOptions.getRowNodeId = function (item) {
            return item.id;
        };
    }

    public onReady(event: GridReadyEvent) {
        this.api = event.api;
        this.columnApi = event.columnApi;
        this.http.fetch("https://raw.githubusercontent.com/ag-grid/ag-grid-docs/master/src/olympicWinners.json",
            {
                method: 'get'
            }).then(result => result.json() as Promise<DataItem[]>).then(
            data => {
                this.loadData(data);
            });
    }

    private loadData(data: DataItem[]) {
        console.log("total data: " + data.length);
        for (let i = 0; i < data.length; i++) {
            data[i].id = i;
        }
        var dataSource = {
            rowCount: 0,
            getRows: (params: IGetRowsParams) => {
                console.log("asking for " + params.startRow + " to " + params.endRow);
                var sortedData = this.sortData(params.sortModel, data);
                var rowsThisPage = sortedData.slice(params.startRow, params.endRow);
                var lastRow = -1;
                if (sortedData.length <= params.endRow) {
                    lastRow = sortedData.length;
                }
                params.successCallback(rowsThisPage, lastRow);
            }
        } as IDatasource;

        this.api.setDatasource(dataSource);
    }

    private sortData(sortModel: any, data: DataItem[]) {
        var sortPresent = sortModel && sortModel.length > 0;
        if (!sortPresent) {
            return data;
        }
        var resultOfSort = data.slice();
        console.log(resultOfSort);
        resultOfSort.sort(function (a: any, b: any) {
            for (var k = 0; k < sortModel.length; k++) {
                var sortColModel = sortModel[k];
                var valueA = a[sortColModel.colId];
                var valueB = b[sortColModel.colId];
                if (valueA == valueB) {
                    continue;
                }
                var sortDirection = sortColModel.sort === "asc" ? 1 : -1;
                if (valueA > valueB) {
                    return sortDirection;
                } else {
                    return sortDirection * -1;
                }
            }
            return 0;
        });
        return resultOfSort;
    }
} 