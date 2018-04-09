///<reference path='../../../node_modules/@types/winrt-uwp/index.d.ts' />
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  public showToast(message, iconUrl) {
    if (typeof Windows !== 'undefined' &&
      typeof Windows.UI !== 'undefined' &&
      typeof Windows.UI.Notifications !== 'undefined') {
      var notifications = Windows.UI.Notifications;
      var template = notifications.ToastTemplateType.toastImageAndText01;
      var toastXml = notifications.ToastNotificationManager.getTemplateContent(template);
      var toastTextElements = toastXml.getElementsByTagName("text");
      toastTextElements[0].appendChild(toastXml.createTextNode(message));
      var toastImageElements = toastXml.getElementsByTagName("image");
      var newAttr = toastXml.createAttribute("src");
      newAttr.value = iconUrl;
      var altAttr = toastXml.createAttribute("alt");
      altAttr.value = "toast graphic";
      var attribs = toastImageElements[0].attributes;
      attribs.setNamedItem(newAttr);
      attribs.setNamedItem(altAttr);
      var toast = new notifications.ToastNotification(toastXml);
      var toastNotifier = notifications.ToastNotificationManager.createToastNotifier();
      toastNotifier.show(toast);
    }
  }


public updateTile(message, imgUrl, imgAlt) {
  if (typeof Windows !== 'undefined' &&
    typeof Windows.UI !== 'undefined' &&
    typeof Windows.UI.Notifications !== 'undefined') {
    var date = new Date();
    var notifications = Windows.UI.Notifications,
      tile = notifications.TileTemplateType.tileSquare150x150PeekImageAndText01,
      tileContent = notifications.TileUpdateManager.getTemplateContent(tile),
      tileText = tileContent.getElementsByTagName('text');
    tileText[0].appendChild(tileContent.createTextNode(message || date.toTimeString()));
    var tileImage = tileContent.getElementsByTagName('image');
    var newAttr = tileContent.createAttribute("src");
    newAttr.value = imgUrl || 'https://unsplash.it/150/150/?random';
    var altAttr = tileContent.createAttribute("alt");
    altAttr.value = imgAlt || 'Random demo image';
    var attribs = tileImage[0].attributes;
    attribs.setNamedItem(newAttr);
    attribs.setNamedItem(altAttr);
    var tileNotification = new notifications.TileNotification(tileContent);
    var currentTime = new Date();
    tileNotification.expirationTime = new Date(currentTime.getTime() + 600 * 1000);
    notifications.TileUpdateManager.createTileUpdaterForApplication().update(tileNotification);
  }
}

}
