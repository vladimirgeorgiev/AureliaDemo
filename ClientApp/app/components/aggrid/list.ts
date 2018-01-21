import { autoinject } from 'aurelia-framework';
import { GridOptions, GridReadyEvent, ColumnApi, GridApi, IDatasource, IGetRowsParams, CellEditingStoppedEvent } from "ag-grid";
import { HttpClient, json } from 'aurelia-fetch-client';
import { DataItem } from './dataItem';
import { PartialMatchFilter } from './partialmatchfilter';

@autoinject
export class List {
    private gridOptions: GridOptions;
    public http: HttpClient;
    private columnApi: ColumnApi;
    private api: GridApi;
    private columnsDefs: any[];
    baseUrl = 'api/SampleData/Athletes';

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
        this.columnsDefs= [
            {
                headerName: "Athlete",
                field: "athlete",
                width: 90,
                filterParams: {
                    filterOptions: ["equals", "lessThan", "greaterThan"],
                    newRowsAction: "keep"
                }
            },
            {
                headerName: "country",
                field: "country",
                width: 90,
                filter: 'agTextColumnFilter',
                filterParams: {
                    newRowsAction: "keep"
                }
            },
            {
                headerName: "sport",
                field: "sport",
                width: 90,
                filter: PartialMatchFilter,
                filterParams: {
                    newRowsAction: "keep"
                }
            }   
        ];
        this.gridOptions.columnDefs = this.columnsDefs;
        this.gridOptions.enableServerSideSorting = true;
        this.gridOptions.enableServerSideFilter = true;
        this.gridOptions.rowModelType = 'infinite';
        this.gridOptions.paginationPageSize = 10;
        this.gridOptions.infiniteInitialRowCount = 1;
        this.gridOptions.maxBlocksInCache = 2;
        this.gridOptions.sortingOrder = ['desc', 'asc'];
        this.gridOptions.getRowNodeId = function (item) {
            return item.id;
        };
        this.gridOptions.onCellValueChanged = (event) => {
            if (event != undefined && event.data != undefined) {
                console.log(event);
                //  update to server
                this.http.fetch(
                    this.baseUrl,
                    {
                        method: 'put',
                        body: json(event.data)
                    });
            }
        }
    }

    public getPartialMatchFilter() {
        return PartialMatchFilter;
    }

    public onReady(event: GridReadyEvent) {
        this.api = event.api;
        this.columnApi = event.columnApi;
        this.loadData();
    }

    private loadData() {
        var dataSource = {
            rowCount: 0,
            getRows: (params: IGetRowsParams) => {

                console.log("asking for " + params.startRow + " to " + params.endRow);
                console.log("filter model: " + JSON.stringify(params.filterModel));

                const propertyNames = Object.getOwnPropertyNames(params.filterModel);
                const filterItems = [];
                for (let i = 0; i < propertyNames.length; i++) {
                    const current = params.filterModel[propertyNames[i]];
                    filterItems.push({
                        filter: current.filter,
                        filterType: current.filterType,
                        type: current.type,
                        filterField: propertyNames[i]
                    });
                }
                
                const fitlerData = {
                    starRow: params.startRow,
                    endRow: params.endRow,
                    colid: params.sortModel.length == 0 ? null : params.sortModel[0].colId,
                    sort: params.sortModel.length == 0 ? null : params.sortModel[0].sort,
                    filters: filterItems
                };

                this.http.fetch(
                    this.baseUrl,
                    {
                        method: 'post',
                        body: json(fitlerData)
                    })
                    .then(result => result.json() as Promise<ResultData>)
                    .then(data => {
                        var rowsThisPage = data.items;
                        var lastRow = -1;
                        if (data.totalCount <= params.endRow) {
                            lastRow = data.totalCount;
                        }
                        params.successCallback(rowsThisPage, lastRow);
                    });
            }
        } as IDatasource;

        this.api.setDatasource(dataSource);
    }
}

export class ResultData {
    totalCount: number;
    items: DataItem[];
}
