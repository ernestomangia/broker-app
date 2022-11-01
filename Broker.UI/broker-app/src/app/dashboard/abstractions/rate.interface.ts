export interface IRateModel {
    date: Date,
    rub: number,
    eur: number,
    gbp: number,
    jpy: number,
    isBuyRate?: boolean
    isSellRate?: boolean
}