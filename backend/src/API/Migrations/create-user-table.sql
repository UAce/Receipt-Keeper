CREATE TABLE IF NOT EXISTS "management"."User"
(
    "Id"         uuid         NOT NULL DEFAULT gen_random_uuid(),
    "FirstName"  varchar(50)  NOT NULL,
    "LastName"   varchar(50)  NOT NULL,
    "Email"      varchar(320) NOT NULL,
    "ExternalId" varchar(36)  NOT NULL,
    CONSTRAINT "user_id_pkey" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "user_external_id_idx" ON "management"."User" ("ExternalId"); 