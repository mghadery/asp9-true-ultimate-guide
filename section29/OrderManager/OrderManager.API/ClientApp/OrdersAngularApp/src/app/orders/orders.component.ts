import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { OrderResponse } from '../models/order-response'
import { OrderEditRequest } from '../models/order-edit-request'
import { OrderListResponse } from '../models/order-list-response';
import { OrdersService } from '../services/orders.service'
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-orders',
  standalone: false,
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {
  orders: OrderResponse[] = [];
  addOrderForm: FormGroup;
  isAddOrderFormSubmitted: boolean = false;
  editOrderForm: FormGroup;
  editOrderId: string | null = null;
  constructor(private ordersService: OrdersService, private accountService: AccountService) {
    this.addOrderForm = new FormGroup({
      customerName: new FormControl(null, [Validators.required])
    });
    this.editOrderForm = new FormGroup({
      orders: new FormArray([])
    })
  }
  get addOrderForm_customerName_control(): any {
    return this.addOrderForm.controls['customerName'];
  }

  get editOrderFormArray(): FormArray {
    return this.editOrderForm.get('orders') as FormArray;
  }

  loadOrders() {
    this.ordersService.getOrders()
      .subscribe({
        next: (
          response: OrderListResponse) => {
          this.orders = response.result;

          this.editOrderFormArray.clear();
          this.orders.forEach(order => {
            this.editOrderFormArray.push(new FormGroup({
              orderId: new FormControl(order.orderId, [Validators.required]),
              customerName: new FormControl({ value: order.customerName, disabled: true }, [Validators.required])
            }))
          })
        },
        error: (error: any) => { console.log(error); },
        complete: () => { }
      }
      );
  }
  ngOnInit() {
    this.loadOrders();
  }

  addOrderSubmitted() {
    this.isAddOrderFormSubmitted = true;
    //console.log("test");
    console.log(this.addOrderForm.value);
    this.ordersService.postOrder(this.addOrderForm.value)
      .subscribe({
        next: (
          response: OrderResponse) => {
          this.loadOrders();
          this.addOrderForm.reset();
          this.isAddOrderFormSubmitted = false;
        },
        error: (error: any) => { console.log(error); },
        complete: () => { }
      }
      );

  }

  editClicked(x:string):void {
    this.editOrderId = x;
  }

  updateClicked(i: number) {
    this.ordersService.putOrder(this.editOrderFormArray.controls[i].value)
      .subscribe({
        next: (
          response: string) => {
          console.log("put success");
          this.editOrderId = null;
          this.editOrderFormArray.controls[i].reset(this.editOrderFormArray.controls[i].value);
        },
        error: (error: any) => { console.log(error); },
        complete: () => { }
      }
    );
  }

  deleteClicked(x: OrderResponse, i: number): void {
    if (confirm(`Are you sure you want to delete the order of customer ${x.customerName}?`)) {
      this.ordersService.delOrder(x.orderId)
        .subscribe({
          next: () => {
            console.log('delete success');
            this.editOrderFormArray.removeAt(i);
            this.orders.splice(i, 1);
          },
          error: (error: any) => { console.log(error); },
          complete: () => { }
        })
    }
  }

  refreshClicked() {
    this.accountService.postRefresh()
      .subscribe({
        next: (response) => {
          localStorage["token"] = response.token;
          localStorage["refreshToken"] = response.refreshToken;          
        },
        error: (error: any) => { console.log(error); },
        complete: () => { }
      });
  }
}
