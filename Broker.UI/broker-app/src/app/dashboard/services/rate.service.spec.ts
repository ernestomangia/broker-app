import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { IBestRevenueRequestModel } from '../abstractions/best-revenue-request.interface';
import { IBestRevenueModel } from '../abstractions/best-revenue.interface';
import { CurrencyCodeType } from '../enums/currency-code-type.enum';

import { RateService } from './rate.service';

describe('RateService', () => {
    let service: RateService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientModule],
            providers: [RateService]
        });
        service = TestBed.inject(RateService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    // TODO: complete this test
    it('#getBestRevenue should return value from observable',
        (done: DoneFn) => {
            let model: IBestRevenueRequestModel = {
                startDate: new Date(Date.UTC(2022, 2, 1)),
                endDate: new Date(Date.UTC(2022, 2, 10)),
                moneyUsd: 100
            };

            service.getBestRevenue(model)
                .subscribe(value => {
                    let expectedData: IBestRevenueModel = {
                        buyDate: new Date(Date.UTC(2022, 2, 4)),
                        sellDate: new Date(Date.UTC(2022, 2, 6)),
                        tool: CurrencyCodeType.RUB,
                        revenue: 4.60,
                        rates: []
                    };

                    expect(value.tool).toBe(expectedData.tool);
                    done();
                });
        });
});
