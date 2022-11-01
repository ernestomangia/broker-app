import { CurrencyCodeType } from "../enums/currency-code-type.enum";
import { IRateModel } from "./rate.interface";

export interface IBestRevenueModel {
    buyDate: Date,
    sellDate: Date,
    tool: CurrencyCodeType,
    revenue: number,
    rates: IRateModel[]
}