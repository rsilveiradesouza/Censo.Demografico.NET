import { Pipe, PipeTransform } from '@angular/core';
enum Regioes {
    'CentroOeste' = 1,
    'Nordeste' = 2,
    'Norte' = 3,
    'Sudeste' = 4,
    'Sul' = 5
}

@Pipe({
    name: 'regioes'
})

export class RegioesPipe implements PipeTransform {

    transform(value: number): any {
        return this.enum(value);
    }

    enum(num: number): string {
        return Regioes[num];
    }

    obterNumerador(str: keyof typeof Regioes): number {
        const descricao = str as keyof typeof Regioes;
        return Regioes[descricao];
    }
}
