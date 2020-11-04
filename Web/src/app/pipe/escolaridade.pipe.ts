import { Pipe, PipeTransform } from '@angular/core';
enum Escolaridade {
    'Fundamental Incompleto' = 1,
    'Fundamental Completo' = 2,
    'Médio Incompleto' = 3,
    'Médio Completo' = 4,
    'Superior Incompleto' = 5,
    'Superior Completo' = 6,
    'Pós Graduado' = 7,
    'Doutorado' = 8,
    'Outro' = 9
}

@Pipe({
    name: 'escolaridade'
})

export class EscolaridadePipe implements PipeTransform {

    transform(value: number): any {
        return this.enum(value);
    }

    enum(num: number): string {
        return Escolaridade[num];
    }

    obterNumerador(str: keyof typeof Escolaridade): number {
        const descricao = str as keyof typeof Escolaridade;
        return Escolaridade[descricao];
    }
}
