import { Pipe, PipeTransform } from '@angular/core';
enum Genero {
    'Masculino' = 1,
    'Feminino' = 2,
    'Indefinido' = 3,
    'Outro' = 4
}

@Pipe({
    name: 'generos'
})

export class GeneroPipe implements PipeTransform {

    transform(value: number): any {
        return this.enum(value);
    }

    enum(num: number): string {
        return Genero[num];
    }

    obterNumerador(str: keyof typeof Genero): number {
        const descricao = str as keyof typeof Genero;
        return Genero[descricao];
    }
}
