import { Injectable } from '@angular/core';
import { OrderAddRequest } from '../models/order-add-request'
import { OrderEditRequest } from '../models/order-edit-request'
import { OrderResponse } from '../models/order-response';
import { OrderListResponse } from '../models/order-list-response';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

const API_BASE_URL: string = "https://localhost:7121/api/ver1/";

@Injectable({
  providedIn: 'root'
})

export class OrdersService {
  constructor(private httpClient : HttpClient) {
  }
  public getOrders(): Observable<OrderListResponse> {
    return this.httpClient.get<OrderListResponse>(`${API_BASE_URL}orders`);
  }
  public postOrder(order: OrderAddRequest): Observable<OrderResponse> {
    return this.httpClient.post<OrderResponse>(`${API_BASE_URL}orders`, order);
  }
  public putOrder(order: OrderEditRequest): Observable<string> {
    return this.httpClient.put<string>(`${API_BASE_URL}orders`, order);
  }
  public delOrder(orderId: string | null): Observable<string> {
    return this.httpClient.delete<string>(`${API_BASE_URL}orders/${orderId}`);
  }
}
