import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { map } from 'rxjs';
import { IBestRevenueModel } from '../abstractions/best-revenue.interface';
import { IRateModel } from '../abstractions/rate.interface';
import { CurrencyCodeType } from '../enums/currency-code-type.enum';
import { RateService } from '../services/rate.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    CurrencyCodeType = CurrencyCodeType;

    displayedColumns: string[] = [
        'date',
        'rub-usd',
        'eur-usd',
        'gbp-usd',
        'jpy-usd'
    ];

    constructor(private _rateService: RateService) {
        var today = new Date();
        this._minDate = new Date(today.setFullYear(today.getFullYear() - 1));
        this._maxDate = new Date();

        this._formGroup = new FormGroup({
            startDate: new FormControl<Date | null>(null, {
                validators: [Validators.required]
            }),
            endDate: new FormControl<Date | null>(null, {
                validators: [Validators.required]
            }),
            moneyUsd: new FormControl(null, {
                validators: [
                    Validators.required,
                    Validators.min(0)
                ]
            })
        });
    }

    private _formGroup: FormGroup;

    get formGroup(): FormGroup {
        return this._formGroup;
    }

    private _minDate: Date;

    get minDate(): Date {
        return this._minDate;
    }

    private _maxDate: Date;

    get maxDate(): Date {
        return this._maxDate;
    }

    private _entity?: IBestRevenueModel;

    get entity(): IBestRevenueModel | undefined {
        return this._entity;
    }

    private _dataSource = new MatTableDataSource<IRateModel>();

    get dataSource(): MatTableDataSource<IRateModel> {
        return this._dataSource;
    }

    ngOnInit(): void {
    }

    onGetBestRevenue(): void {
        if (!this._formGroup.valid)
            return;

        var model = this._formGroup.getRawValue();

        this._rateService
            .getBestRevenue(model)
            .pipe(
                map((data: IBestRevenueModel) => {
                    let buyRate = data.rates.find(e => e.date == data.buyDate);
                    let sellRate = data.rates.find(e => e.date == data.sellDate);

                    if (buyRate)
                        buyRate.isBuyRate = true;

                    if (sellRate)
                        sellRate.isSellRate = true;

                    return data;
                })
            )
            .subscribe((data: IBestRevenueModel) => {
                this._entity = data;

                this.dataSource.data = data.rates;
                this.dataSource._updateChangeSubscription();
            });
    }
}
