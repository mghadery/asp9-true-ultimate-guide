import { Injectable } from '@angular/core';
import { OrderAddRequest } from '../models/order-add-request'
import { OrderEditRequest } from '../models/order-edit-request'
import { OrderResponse } from '../models/order-response';
import { OrderListResponse } from '../models/order-list-response';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';


const API_BASE_URL: string = "https://localhost:7121/api/ver1/";

@Injectable({
  providedIn: 'root'
})

export class OrdersService {
  constructor(private httpClient : HttpClient) {
  }
  public getOrders(): Observable<OrderListResponse> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage["token"]}`);
    return this.httpClient.get<OrderListResponse>(`${API_BASE_URL}orders`, { headers: headers });
  }
  public postOrder(order: OrderAddRequest): Observable<OrderResponse> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage["token"]}`);
    return this.httpClient.post<OrderResponse>(`${API_BASE_URL}orders`, order, { headers: headers });
  }
  public putOrder(order: OrderEditRequest): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage["token"]}`);
    return this.httpClient.put<string>(`${API_BASE_URL}orders`, order, { headers: headers });
  }
  public delOrder(orderId: string | null): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage["token"]}`);
    return this.httpClient.delete<string>(`${API_BASE_URL}orders/${orderId}`, { headers: headers });
  }
}
