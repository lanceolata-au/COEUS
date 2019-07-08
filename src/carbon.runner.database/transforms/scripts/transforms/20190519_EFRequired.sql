/* This is needed so that entity framework is able to complete its migrations for IdentityServer4 */
CREATE TABLE __EFMigrationsHistory(
  MigrationId VARCHAR(150) NOT NULL,
  ProductVersion VARCHAR(32) NOT NULL
);