import { formatDate } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IBestRevenueRequestModel } from '../abstractions/best-revenue-request.interface';
import { IBestRevenueModel } from '../abstractions/best-revenue.interface';

@Injectable({
  providedIn: 'root'
})
export class RateService {
  private baseUrl: string = environment.api.baseUrl;
  private path = "rates";

  constructor(private _httpClient: HttpClient) { }

  getBestRevenue(model: IBestRevenueRequestModel): Observable<IBestRevenueModel> {
    return this._httpClient.get<IBestRevenueModel>(`${this.baseUrl}/${this.path}/best`, {
      params: {
        startDate: formatDate(model.startDate, 'yyyy-M-d', "en-US"),
        endDate: formatDate(model.endDate, 'yyyy-M-d', "en-US"),
        moneyUsd: model.moneyUsd
      }
    });
  }
}
