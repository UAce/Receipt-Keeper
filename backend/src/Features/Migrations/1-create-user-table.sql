CREATE TABLE IF NOT EXISTS "User" (
    "Id" uuid NOT NULL DEFAULT gen_random_uuid (),
    "FirstName" varchar(50) NOT NULL,
    "LastName" varchar(50) NOT NULL,
    "Email" varchar(320) NOT NULL,
    "IdentityId" varchar(36) NOT NULL,
    CONSTRAINT "user_id_pkey" PRIMARY KEY ("Id")
);

-- We create an index on the IdentityId column to speed up queries that search for a user by their IdentityId.
CREATE UNIQUE INDEX IF NOT EXISTS "user_external_id_idx" ON "User" ("IdentityId");