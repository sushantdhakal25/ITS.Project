
import { throwError as observableThrowError, Observable, throwError } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, finalize, map } from 'rxjs/operators';
import { ItsHttpOptions } from '../shared/models/http-options.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';

interface CJSON {
  expand(value: any): any;
}

declare const window: Window & { CJSON: CJSON };

@Injectable(
  {
    providedIn: 'root'
  }
)
export class WebApiService {

  private spinnerDuration = 700;
  constructor(
    public http: HttpClient,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {
  }


  public get<T>(url: string, params?: any, toastMessage?: string, showBlockUI: boolean = true, showDefaultToaster: boolean = true): Observable<T> {
    if (showBlockUI) {
       this.spinner.show();
    }

    return this.http.get<T>(url, { params: params }).pipe(
      map(data => this.unwrapHttpValue(data)),
      catchError((error: any) => {
        this.errorMessage(error, showDefaultToaster);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  public getWithOptions<T>(url: string, options?: ItsHttpOptions, showBlockUI: boolean = true): Observable<T> {
    if (showBlockUI) {
      this.spinner.show();
    }

    return this.http.get<T>(url, options).pipe(
      map(data => {
        if (options && options.notCamalizedJson) {
          return data;
        } else {
          return this.unwrapHttpValue(data);
        }
      }),
      catchError((error: any) => {
        this.errorMessage(error);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  public post<T>(url: string, data?: any, params?: any, toastMessage?: string, showBlockUI: boolean = true): Observable<T> {
    if (showBlockUI) {
      this.spinner.show();
    }
    // for removing space of data when data is not null
    if (data != null) {
      data = JSON.stringify(data).replace(/"\s+|\s+"/g, '"');
      data = JSON.parse(data);
    }

    return this.http.post<T>(url, data).pipe(
      map(res => this.unwrapHttpValue(res)),
      catchError((error: any) => {
        this.errorMessage(error);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  public postWithOptions<T>(url: string, body: any, options?: ItsHttpOptions, showBlockUI: boolean = true): Observable<T> {
    if (showBlockUI) {
      this.spinner.show();
    }

    return this.http.post<T>(url, body, options).pipe(
      map(data => {
        if (options && options.notCamalizedJson) {

          return data;
        } else {

          return this.unwrapHttpValue(data);
        }
      }),
      catchError((error: any) => {
        this.errorMessage(error);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  public put<T>(url: string, data?: any, params?: any, toastMessage?: string, showBlockUI: boolean = true): Observable<T> {
    if (showBlockUI) {
      this.spinner.show();
    }

    return this.http.put<T>(url, data).pipe(
      catchError((error: any) => {
        this.errorMessage(error);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  public putWithOptions<T>(url: string, body?: any, options?: ItsHttpOptions, showBlockUI: boolean = true): Observable<T> {
    if (showBlockUI) {
      this.spinner.show();
    }

    return this.http.put<T>(url, body, options).pipe(
      catchError((error: any) => {
        this.errorMessage(error);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  public delete<T>(url: string, data?: any, showBlockUI: boolean = true): Observable<T> {
    if (showBlockUI) {
      this.spinner.show();
    }

    return this.http.delete<T>(url, {body: data}).pipe(
      catchError((error: any) => {
        this.errorMessage(error);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  public deleteWithOptions<T>(url: string, options: ItsHttpOptions, showBlockUI: boolean = true): Observable<T> {
    if (showBlockUI) {
      this.spinner.show();
    }

    return this.http.delete<T>(url, options).pipe(
      catchError((error: any) => {
        this.errorMessage(error);
        return throwError(() => this.unwrapHttpValue(error));
      }),
      finalize(() => {
        if (showBlockUI) {
          setTimeout(() =>{
            this.spinner.hide()
          }, this.spinnerDuration)
        }
      })
    );
  }

  private buildUrlSearchParams(params: any): HttpParams {
    const searchParams = new HttpParams();
    for (const key in params) {
      if (params.hasOwnProperty(key)) {
        searchParams.append(key, params[key]);
      }
    }
    return searchParams;
  }

  private unwrapHttpValue(value: any): any {
    if (value === null) {
      return value;
    }

    if (typeof value === 'string') {
      value = JSON.parse(value);
    }

    if (value['f'] === 'cjson') {
      value = window['CJSON'].expand(value);
    }
    return (this.toCamel(value));
  }
  
  toCamel(o: any) : any {
    let newO : any, origKey, newKey, value;
    if (!o) {
      return null;
    }

    if (o instanceof Array) {

      newO = [];
      for (origKey in o) {

        if (o.hasOwnProperty(origKey)) {

          value = o[origKey];
          if (typeof value === 'object') {

            value = this.toCamel(value);
          }

          newO.push(value);
        }
      }
    } else {
      newO = {};
      for (origKey in o) {

        if (o.hasOwnProperty(origKey)) {

          newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString()
          value = o[origKey];

          if (value instanceof Array || (value !== null && value.constructor === Object)) {
            value = this.toCamel(value);
          }

          newO[newKey] = value;
        }
      }
    }
    return newO;
  }
  
 

  private errorMessage(error: any, showDefaultToaster: boolean = true): void {
    let finalMessage = 'Error response received';
    if (error && !this.isUnauthorize(error) && showDefaultToaster) {
      if (error.error) {
        if (typeof error.error === 'string') {
          try {
            error.error = JSON.parse(error.error);
          } catch (e) {
          }
        }

        if (error.error.errors?.constructor === Array) {
          finalMessage = error.error.errors[0].message;
        }
        else if (error.error.error?.constructor === String) {
          finalMessage = error.error.error;
        }

      }
      else {
        finalMessage = error.statusText;
      }

      alert(finalMessage);
    }
  }

  private isUnauthorize(errRes: any) {
    let errorHandled = false;
    const isUnauthorize = errRes.status === 401;
    if (isUnauthorize) {
      //should redirect to login
      errorHandled = true;
    }
    return errorHandled;
  }
}
