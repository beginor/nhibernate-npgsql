CREATE TABLE public.test_table
(
    id serial NOT NULL,
    name character varying(32) NOT NULL,
    tags character varying(32)[],
    json_field json,
    jsonb_field jsonb,
    update_time timestamp without time zone,
    CONSTRAINT pk_test_table PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.test_table
    OWNER to postgres;
COMMENT ON TABLE public.test_table
    IS 'test table';