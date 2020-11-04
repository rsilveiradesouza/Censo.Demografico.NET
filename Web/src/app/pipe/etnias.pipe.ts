import { Pipe, PipeTransform } from '@angular/core';
enum Etnia {
    'Negro' = 1,
    'Branco' = 2,
    'Pardo' = 3,
    'Indigena' = 4,
    'Caboclo' = 5,
    'Mulato' = 6,
    'Cafuzo' = 7
}

@Pipe({
    name: 'etnia'
})

export class EtniaPipe implements PipeTransform {

    transform(value: number): any {
        return this.enum(value);
    }

    enum(num: number): string {
        return Etnia[num];
    }

    obterNumerador(str: keyof typeof Etnia): number {
        const descricao = str as keyof typeof Etnia;
        return Etnia[descricao];
    }
}
