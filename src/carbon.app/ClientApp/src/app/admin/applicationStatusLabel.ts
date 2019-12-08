export enum ApplicationStatus {
  Excluded = 0,
  Canceled = 10,
  Preliminary = 15,
  Started = 20,
  ReadyToSubmit = 50,
  Submitted = 80,
  Approved = 100,
}

export const applicationStatusLabel = new Map<number, string>([
  [ApplicationStatus.Excluded, 'Excluded'],
  [ApplicationStatus.Canceled, 'Canceled'],
  [ApplicationStatus.Preliminary, 'Preliminary'],
  [ApplicationStatus.Started, 'Started'],
  [ApplicationStatus.ReadyToSubmit, 'Ready to Submit'],
  [ApplicationStatus.Submitted, 'Submitted'],
  [ApplicationStatus.Approved, 'Approved']
]);
