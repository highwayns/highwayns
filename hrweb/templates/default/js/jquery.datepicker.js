
Date.dayNames = ['日', '一', '二', '三', '四', '五', '六'];

Date.abbrDayNames = ['日', '一', '二', '三', '四', '五', '六'];

Date.monthNames = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];

Date.abbrMonthNames = ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '十一', '十二'];

Date.firstDayOfWeek = 1;

Date.format = 'yyyy-mm-dd';
//Date.format = 'mm/dd/yyyy';
//Date.format = 'yyyy-mm-dd';
//Date.format = 'dd mmm yy';

Date.fullYearStart = '20';

(function() {

  function add(name, method) {
    if( !Date.prototype[name] ) {
      Date.prototype[name] = method;
    }
  };
  
  add("isLeapYear", function() {
    var y = this.getFullYear();
    return (y%4==0 && y%100!=0) || y%400==0;
  });
  
  add("isWeekend", function() {
    return this.getDay()==0 || this.getDay()==6;
  });
  
  add("isWeekDay", function() {
    return !this.isWeekend();
  });
  
  add("getDaysInMonth", function() {
    return [31,(this.isLeapYear() ? 29:28),31,30,31,30,31,31,30,31,30,31][this.getMonth()];
  });
  
  
  add("getLastDayInMonth", function() {
    this.setDate(this.getDaysInMonth());
    return this;
  });
  
  add("getFirstDayInMonth", function() {
    this.setDate(1);
    return this;
  });
  
  add("getDayName", function(abbreviated) {
    return abbreviated ? Date.abbrDayNames[this.getDay()] : Date.dayNames[this.getDay()];
  });

  add("getMonthName", function(abbreviated) {
    return abbreviated ? Date.abbrMonthNames[this.getMonth()] : Date.monthNames[this.getMonth()];
  });

  add("getDayOfYear", function() {
    var tmpdtm = new Date("1/1/" + this.getFullYear());
    return Math.floor((this.getTime() - tmpdtm.getTime()) / 86400000);
  });
  
  add("getWeekOfYear", function() {
    return Math.ceil(this.getDayOfYear() / 7);
  });

  add("setDayOfYear", function(day) {
    this.setMonth(0);
    this.setDate(day);
    return this;
  });
  
  add("addYears", function(num) {
    this.setFullYear(this.getFullYear() + num);
    return this;
  });
  
  add("addMonths", function(num) {
    var tmpdtm = this.getDate();
    
    this.setMonth(this.getMonth() + num);
    
    if (tmpdtm > this.getDate())
      this.addDays(-this.getDate());
    
    return this;
  });
  
  add("addDays", function(num) {
    //this.setDate(this.getDate() + num);
    this.setTime(this.getTime() + (num*86400000) );
    return this;
  });
  
  add("addHours", function(num) {
    this.setHours(this.getHours() + num);
    return this;
  });

  add("addMinutes", function(num) {
    this.setMinutes(this.getMinutes() + num);
    return this;
  });
  
  add("addSeconds", function(num) {
    this.setSeconds(this.getSeconds() + num);
    return this;
  });
  
  add("zeroTime", function() {
    this.setMilliseconds(0);
    this.setSeconds(0);
    this.setMinutes(0);
    this.setHours(0);
    return this;
  });
  
  add("asString", function(format) {
    var r = format || Date.format;
    return r
      .split('yyyy').join(this.getFullYear())
      .split('yy').join((this.getFullYear() + '').substring(2))
      .split('mmmm').join(this.getMonthName(false))
      .split('mmm').join(this.getMonthName(true))
      .split('mm').join(_zeroPad(this.getMonth()+1))
      .split('dd').join(_zeroPad(this.getDate()))
      .split('hh').join(_zeroPad(this.getHours()))
      .split('min').join(_zeroPad(this.getMinutes()))
      .split('ss').join(_zeroPad(this.getSeconds()));
  });
  
  Date.fromString = function(s, format)
  {
    var f = format || Date.format;
    var d = new Date('01/01/1977');
    
    var mLength = 0;

    var iM = f.indexOf('mmmm');
    if (iM > -1) {
      for (var i=0; i<Date.monthNames.length; i++) {
        var mStr = s.substr(iM, Date.monthNames[i].length);
        if (Date.monthNames[i] == mStr) {
          mLength = Date.monthNames[i].length - 4;
          break;
        }
      }
      d.setMonth(i);
    } else {
      iM = f.indexOf('mmm');
      if (iM > -1) {
        var mStr = s.substr(iM, 3);
        for (var i=0; i<Date.abbrMonthNames.length; i++) {
          if (Date.abbrMonthNames[i] == mStr) break;
        }
        d.setMonth(i);
      } else {
        d.setMonth(Number(s.substr(f.indexOf('mm'), 2)) - 1);
      }
    }
    
    var iY = f.indexOf('yyyy');

    if (iY > -1) {
      if (iM < iY)
      {
        iY += mLength;
      }
      d.setFullYear(Number(s.substr(iY, 4)));
    } else {
      if (iM < iY)
      {
        iY += mLength;
      }
      // TODO - this doesn't work very well - are there any rules for what is meant by a two digit year?
      d.setFullYear(Number(Date.fullYearStart + s.substr(f.indexOf('yy'), 2)));
    }
    var iD = f.indexOf('dd');
    if (iM < iD)
    {
      iD += mLength;
    }
    d.setDate(Number(s.substr(iD, 2)));
    if (isNaN(d.getTime())) {
      return false;
    }
    return d;
  };
  
  // utility method
  var _zeroPad = function(num) {
    var s = '0'+num;
    return s.substring(s.length-2)
    //return ('0'+num).substring(-2); // doesn't work on IE :(
  };
  
})();

