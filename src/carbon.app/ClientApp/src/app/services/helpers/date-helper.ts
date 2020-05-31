export class DateHelper {

  static mootStart = new Date(2022, 12, 31, 0);

  constructor() {
  }

  public static dateReformat(dateContainer) {
    let dob = new Date(dateContainer.dateOfBirth);

    dateContainer.applicationAgeAtMoot = DateHelper.daysBetween(dob, DateHelper.mootStart);

    dateContainer.dateOfBirth = dob.toDateString()

  }


  public static daysBetween(date1, date2) {
    //Get 1 day in milliseconds
    let one_day=1000*60*60*24;

    // Convert both dates to milliseconds
    let date1_ms = date1.getTime();
    let date2_ms = date2.getTime();

    // Calculate the difference in milliseconds
    let difference_ms = date2_ms - date1_ms;

    let days = Math.floor(difference_ms/one_day) - 30;

    let years = Number.parseInt((days/365.25).toFixed(2));

    let months = Math.floor((days/365.25 - years) * 12);

    // Convert back to days and return
    return {years, months}
  }

}
