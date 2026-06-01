import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  return next(request).pipe(
    catchError((error: HttpErrorResponse) => {
      const message =
        error.error?.error ??
        error.error?.message ??
        (error.status === 0
          ? 'Nao foi possivel conectar com a API.'
          : 'Ocorreu um erro inesperado.');

      return throwError(() => new Error(message));
    })
  );
};
