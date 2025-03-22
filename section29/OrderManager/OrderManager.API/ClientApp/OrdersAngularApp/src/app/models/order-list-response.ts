import { OrderResponse } from "./order-response";

export class OrderListResponse {
  totalCount: number;
  pageSize: number;
  pageNumber: number;
  count: number;
  result: OrderResponse[];
  constructor(totalCount: number,
    pageSize: number,
    pageNumber: number,
    count: number,
    result: OrderResponse[]) {
    this.count = count;
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.totalCount = totalCount;
    this.result = result;
    }
}


//public int TotalCount { get; set; }
//    public int PageSize { get; set; }
//    public int PageNumber { get; set; }
//    public int Count { get; set; }
