import { Component, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, debounceTime, distinctUntilChanged, of, switchMap, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Cliente {
  id: number;
  tipoDocumento: string;
  numeroDocumento: string;
  razonSocial: string;
  segmento: string;
  correo: string;
}

@Component({
  selector: 'app-clientes-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './clientes.page.html'
})
export class ClientesPage {
  private readonly http = inject(HttpClient);

  readonly busqueda = new FormControl('', { nonNullable: true });
  readonly seleccionado = signal<Cliente | null>(null);
  readonly error = signal('');

  readonly resultados = toSignal(
    this.busqueda.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(q => {
        const termino = q.trim();
        this.error.set('');
        if (termino.length < 3) return of<Cliente[]>([]);
        return this.http
          .get<Cliente[]>(`${environment.apiUrl}/clientes/buscar`, { params: { q: termino, limit: 10 } })
          .pipe(
            catchError(() => {
              this.error.set('No se pudo realizar la búsqueda.');
              return of<Cliente[]>([]);
            })
          );
      }),
      tap(() => this.seleccionado.set(null))
    ),
    { initialValue: [] as Cliente[] }
  );
}
