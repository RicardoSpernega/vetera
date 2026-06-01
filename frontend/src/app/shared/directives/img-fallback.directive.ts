import { Directive, HostListener, Input } from '@angular/core';

@Directive({
  selector: 'img[appImgFallback]',
  standalone: true
})
export class ImgFallbackDirective {
  @Input() appImgFallback = 'assets/images/image-fallback.svg';

  @HostListener('error', ['$event.target'])
  onError(target: HTMLImageElement): void {
    target.src = this.appImgFallback;
  }
}
